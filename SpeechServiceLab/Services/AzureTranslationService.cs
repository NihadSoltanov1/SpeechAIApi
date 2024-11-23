using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SpeechServiceLab.Services
{
    public class AzureTranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _subscriptionKey;
        private readonly string _region;

        public AzureTranslationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _subscriptionKey = configuration["AzureSpeechService:SubscriptionKey"];
            _region = configuration["AzureSpeechService:Region"];
        }

        public async Task<string> TranslateTextAsync(string text, string toLanguage)
        {
            var endpoint = $"https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&to={toLanguage}";
            var body = JsonSerializer.Serialize(new[] { new { Text = text } });
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", _region);

            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
