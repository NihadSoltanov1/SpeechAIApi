using Microsoft.AspNetCore.Mvc;
using SpeechServiceLab.Services;
using SpeechServiceLab.Models;
using System.IO;
using System.Threading.Tasks;

namespace SpeechServiceLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechController : ControllerBase
    {
        private readonly AzureSpeechService _speechService;
        private readonly AzureTranslationService _translationService;
        private readonly AzureSpeakerRecognitionService _speakerService;

        public SpeechController(
            AzureSpeechService speechService,
            AzureTranslationService translationService,
            AzureSpeakerRecognitionService speakerService)
        {
            _speechService = speechService;
            _translationService = translationService;
            _speakerService = speakerService;
        }

        [HttpPost("text-to-speech")]
        public async Task<IActionResult> TextToSpeech([FromBody] string text)
        {
            var audioData = await _speechService.TextToSpeechAsync(text);
            return File(audioData, "audio/wav", "output.wav");
        }

        [HttpPost("speech-to-text")]
        public async Task<IActionResult> SpeechToText(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("Audio file is required.");
            }

            using var memoryStream = new MemoryStream();
            await audioFile.CopyToAsync(memoryStream);

            try
            {
                var result = await _speechService.SpeechToTextAsync(memoryStream.ToArray());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Speech-to-Text işlemi sırasında hata oluştu: {ex.Message}");
            }
        }


        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            var translatedText = await _translationService.TranslateTextAsync(request.Text, request.ToLanguage);
            return Ok(translatedText);
        }

        [HttpPost("recognize-speaker")]
        public async Task<IActionResult> RecognizeSpeaker(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("Audio file is required.");
            }

            using var memoryStream = new MemoryStream();
            await audioFile.CopyToAsync(memoryStream);

            try
            {
                var result = await _speakerService.RecognizeSpeakerAsync(memoryStream.ToArray());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Speaker Recognition failed: {ex.Message}");
            }
        }

    }
}
