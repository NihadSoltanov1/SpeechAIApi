using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SpeechServiceLab.Services
{
    public class AzureSpeechService
    {
        private readonly HttpClient _httpClient;
        private readonly string _subscriptionKey;
        private readonly string _region;

        public AzureSpeechService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _subscriptionKey = configuration["AzureSpeechService:SubscriptionKey"];
            _region = configuration["AzureSpeechService:Region"];
        }

        // Text-to-Speech (TTS)
        public async Task<byte[]> TextToSpeechAsync(string text)
        {
            var endpoint = $"https://{_region}.tts.speech.microsoft.com/cognitiveservices/v1";

            var ssml = $@"
    <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
        <voice name='en-US-JennyNeural'>{text}</voice>
    </speak>";

            var content = new StringContent(ssml, Encoding.UTF8, "application/ssml+xml");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            _httpClient.DefaultRequestHeaders.Add("X-MICROSOFT-OutputFormat", "riff-16khz-16bit-mono-pcm");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "SpeechServiceLab/1.0"); // Zorunlu başlık

            try
            {
                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"HTTP Yanıt İçeriği: {responseContent}");

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                throw;
            }
        }


        // Speech-to-Text (STT)
        public async Task<string> SpeechToTextAsync(byte[] audioData)
        {
            var endpoint = $"https://{_region}.stt.speech.microsoft.com/speech/recognition/conversation/cognitiveservices/v1?language=en-US";

            var content = new ByteArrayContent(audioData);
            content.Headers.Add("Content-Type", "audio/wav");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
