using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;

namespace SpeechServiceLab.Services
{
    public class AzureSpeakerRecognitionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _subscriptionKey;
        private readonly string _endpoint;

        public AzureSpeakerRecognitionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _subscriptionKey = configuration["AzureSpeechService:SubscriptionKey"];
            _endpoint = configuration["AzureSpeechService:Endpoint"]; // Ensure this is an absolute URI
        }

        public async Task<string> RecognizeSpeakerAsync(byte[] audioData)
        {
            // Construct the full absolute URL
            var url = $"{_endpoint}/speaker/recognition/v1.0/identify";

            var content = new ByteArrayContent(audioData);
            content.Headers.Add("Content-Type", "audio/wav");

            // Add required headers
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            try
            {
                Console.WriteLine($"Sending request to URL: {url}");
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Speaker Recognition failed: {ex.Message}");
            }
        }
    }
}
