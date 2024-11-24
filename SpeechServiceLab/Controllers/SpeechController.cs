using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpeechServiceLab.Models;
using SpeechServiceLab.Services;

namespace SpeechServiceLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechController : ControllerBase
    {
        private readonly AzureSpeechService _speechService;
        private readonly AzureTranslationService _translationService;

        // Request zamanları ve ID'lerini saklama
        private static readonly ConcurrentDictionary<string, (DateTime lastRequestTime, string lastRequestId)> RequestInfo = new();
        private static readonly ConcurrentDictionary<string, ProcessState> StateStore = new(); // Eksikse ekledik
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(30);
        private static readonly object _requestLock = new(); // Senkronizasyon için referans türü

        public SpeechController(
            AzureSpeechService speechService,
            AzureTranslationService translationService)
        {
            _speechService = speechService;
            _translationService = translationService;
        }

        private (bool canProcess, string lastRequestId) CanProcessRequest(string key)
        {
            lock (_requestLock)
            {
                if (RequestInfo.TryGetValue(key, out var requestInfo))
                {
                    if (DateTime.UtcNow - requestInfo.lastRequestTime < RequestTimeout)
                    {
                        return (false, requestInfo.lastRequestId);
                    }
                }

                var newRequestId = Guid.NewGuid().ToString();
                RequestInfo[key] = (DateTime.UtcNow, newRequestId); // Yeni zaman ve ID kaydediliyor
                return (true, newRequestId);
            }
        }

        [HttpPost("speech-to-text")]
        public async Task<IActionResult> SpeechToText(IFormFile audioFile)
        {
            var (canProcess, lastRequestId) = CanProcessRequest("speech-to-text");
            if (!canProcess)
            {
                return BadRequest($"You must wait 30 seconds between requests for speech-to-text. Last request ID: {lastRequestId}");
            }

            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("Audio file is required.");
            }

            var requestId = Guid.NewGuid().ToString();
            using var memoryStream = new MemoryStream();
            await audioFile.CopyToAsync(memoryStream);

            _ = Task.Run(async () =>
            {
                try
                {
                    var result = await _speechService.SpeechToTextAsync(memoryStream.ToArray());
                    StateStore[requestId] = new ProcessState { Status = "Completed", Result = result };
                }
                catch (Exception ex)
                {
                    StateStore[requestId] = new ProcessState { Status = "Failed", Result = ex.Message };
                }
            });

            return Ok(new { RequestId = requestId });
        }

        [HttpPost("text-to-speech")]
        public async Task<IActionResult> TextToSpeech([FromBody] string text)
        {
            var (canProcess, lastRequestId) = CanProcessRequest("text-to-speech");
            if (!canProcess)
            {
                return BadRequest($"You must wait 30 seconds between requests for text-to-speech. Last request ID: {lastRequestId}");
            }

            var requestId = Guid.NewGuid().ToString();
            _ = Task.Run(async () =>
            {
                try
                {
                    var audioData = await _speechService.TextToSpeechAsync(text);
                    StateStore[requestId] = new ProcessState { Status = "Completed", AudioData = audioData };
                }
                catch (Exception ex)
                {
                    StateStore[requestId] = new ProcessState { Status = "Failed", Result = ex.Message };
                }
            });

            return Ok(new { RequestId = requestId });
        }

        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            var (canProcess, lastRequestId) = CanProcessRequest("translate");
            if (!canProcess)
            {
                return BadRequest($"You must wait 30 seconds between requests for translation. Last request ID: {lastRequestId}");
            }

            var requestId = Guid.NewGuid().ToString();
            _ = Task.Run(async () =>
            {
                try
                {
                    var result = await _translationService.TranslateTextAsync(request.Text, request.ToLanguage);
                    StateStore[requestId] = new ProcessState { Status = "Completed", Result = result };
                }
                catch (Exception ex)
                {
                    StateStore[requestId] = new ProcessState { Status = "Failed", Result = ex.Message };
                }
            });

            return Ok(new { RequestId = requestId });
        }

        [HttpGet("status/{requestId}")]
        public IActionResult GetStatus(string requestId)
        {
            if (!StateStore.TryGetValue(requestId, out var state))
            {
                return NotFound("Request ID not found.");
            }
            return Ok(state);
        }

        [HttpGet("audio/{requestId}")]
        public IActionResult GetAudio(string requestId)
        {
            if (!StateStore.TryGetValue(requestId, out var state) || state.AudioData == null)
            {
                return BadRequest("Audio file is not ready.");
            }

            return File(state.AudioData, "audio/wav", "output.wav");
        }
    }

    public class ProcessState
    {
        public string Status { get; set; }
        public string Result { get; set; }
        public byte[] AudioData { get; set; }
    }
}
