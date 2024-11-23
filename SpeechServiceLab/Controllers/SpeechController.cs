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

        [HttpPost("speech-to-text")]
        public async Task<IActionResult> SpeechToText(IFormFile audioFile)
        {
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
