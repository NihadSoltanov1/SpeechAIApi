using Microsoft.AspNetCore.Mvc;
using SpeechServiceLab.Services;
using SpeechServiceLab.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpeechServiceLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechController : ControllerBase
    {
        private readonly AzureSpeechService _speechService;
        private readonly AzureTranslationService _translationService;

        private static readonly ConcurrentDictionary<string, ProcessState> StateStore = new();
        private static readonly ConcurrentDictionary<string, string> HashToRequestIdMap = new();
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(30);

        public SpeechController(
            AzureSpeechService speechService,
            AzureTranslationService translationService)
        {
            _speechService = speechService;
            _translationService = translationService;
        }

        private string ComputeHash(IFormFile file)
        {
            using var md5 = MD5.Create();
            using var stream = file.OpenReadStream();
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        private string ComputeHash(string text)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        [HttpPost("speech-to-text")]
        public async Task<IActionResult> SpeechToText(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("Audio file is required.");
            }

            var fileHash = ComputeHash(audioFile);

            if (HashToRequestIdMap.TryGetValue(fileHash, out var existingRequestId) &&
                StateStore.TryGetValue(existingRequestId, out var existingState) &&
                DateTime.UtcNow - existingState.Timestamp <= RequestTimeout)
            {
                return Ok(new
                {
                    Message = "This file is already being processed. Please wait.",
                    RequestId = existingRequestId
                });
            }

            var requestId = Guid.NewGuid().ToString();
            HashToRequestIdMap[fileHash] = requestId;
            StateStore[requestId] = new ProcessState
            {
                RequestId = requestId,
                Status = "Processing",
                Timestamp = DateTime.UtcNow
            };

            using var memoryStream = new MemoryStream();
            await audioFile.CopyToAsync(memoryStream);

            _ = Task.Run(async () =>
            {
                try
                {
                    var result = await _speechService.SpeechToTextAsync(memoryStream.ToArray());
                    StateStore[requestId].Status = "Completed";
                    StateStore[requestId].Result = result;
                }
                catch (Exception ex)
                {
                    StateStore[requestId].Status = "Failed";
                    StateStore[requestId].Result = ex.Message;
                }
            });

            return Ok(new { RequestId = requestId });
        }

        [HttpPost("text-to-speech")]
        public async Task<IActionResult> TextToSpeech([FromBody] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("Text is required.");
            }

            var textHash = ComputeHash(text);

            if (HashToRequestIdMap.TryGetValue(textHash, out var existingRequestId) &&
                StateStore.TryGetValue(existingRequestId, out var existingState) &&
                DateTime.UtcNow - existingState.Timestamp <= RequestTimeout)
            {
                return Ok(new
                {
                    Message = "This text is already being processed. Please wait.",
                    RequestId = existingRequestId
                });
            }

            var requestId = Guid.NewGuid().ToString();
            HashToRequestIdMap[textHash] = requestId;
            StateStore[requestId] = new ProcessState
            {
                RequestId = requestId,
                Status = "Processing",
                Timestamp = DateTime.UtcNow
            };

            _ = Task.Run(async () =>
            {
                try
                {
                    var audioData = await _speechService.TextToSpeechAsync(text);
                    StateStore[requestId].Status = "Completed";
                    StateStore[requestId].AudioData = audioData;
                }
                catch (Exception ex)
                {
                    StateStore[requestId].Status = "Failed";
                    StateStore[requestId].Result = ex.Message;
                }
            });

            return Ok(new { RequestId = requestId });
        }

        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text) || string.IsNullOrWhiteSpace(request.ToLanguage))
            {
                return BadRequest("Text and target language are required.");
            }

            var textHash = ComputeHash($"{request.Text}-{request.ToLanguage}");

            if (HashToRequestIdMap.TryGetValue(textHash, out var existingRequestId) &&
                StateStore.TryGetValue(existingRequestId, out var existingState) &&
                DateTime.UtcNow - existingState.Timestamp <= RequestTimeout)
            {
                return Ok(new
                {
                    Message = "This translation is already being processed. Please wait.",
                    RequestId = existingRequestId
                });
            }

            var requestId = Guid.NewGuid().ToString();
            HashToRequestIdMap[textHash] = requestId;
            StateStore[requestId] = new ProcessState
            {
                RequestId = requestId,
                Status = "Processing",
                Timestamp = DateTime.UtcNow
            };

            _ = Task.Run(async () =>
            {
                try
                {
                    var translatedText = await _translationService.TranslateTextAsync(request.Text, request.ToLanguage);
                    StateStore[requestId].Status = "Completed";
                    StateStore[requestId].Result = translatedText;
                }
                catch (Exception ex)
                {
                    StateStore[requestId].Status = "Failed";
                    StateStore[requestId].Result = ex.Message;
                }
            });

            return Ok(new { RequestId = requestId });
        }

        [HttpGet("status/{requestId}")]
        public IActionResult GetStatus(string requestId)
        {
            if (!StateStore.ContainsKey(requestId))
            {
                return NotFound("Request ID not found.");
            }

            var state = StateStore[requestId];
            return Ok(state);
        }

        [HttpGet("audio/{requestId}")]
        public IActionResult GetAudio(string requestId)
        {
            if (!StateStore.ContainsKey(requestId))
            {
                return NotFound("Request ID not found.");
            }

            var state = StateStore[requestId];
            if (state.Status != "Completed" || state.AudioData == null)
            {
                return BadRequest("Audio file is not ready.");
            }

            return File(state.AudioData, "audio/wav", "output.wav");
        }
    }

    public class ProcessState
    {
        public string RequestId { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public byte[] AudioData { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
