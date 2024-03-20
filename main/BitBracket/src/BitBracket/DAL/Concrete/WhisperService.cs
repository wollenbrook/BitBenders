using BitBracket.DAL.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BitBracket.DAL.Concrete
{
    public class WhisperService : IWhisperService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WhisperService> _logger;
        private readonly string _apiKey;

        public WhisperService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<WhisperService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _apiKey = configuration["BitBracketOpenAI"];

            // Set up the HttpClient instance with the OpenAI API base URL and authentication headers.
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> TranscribeAudioAsync(Stream audioStream, string languageCode)
        {
            try
            {
                // OpenAI API endpoint.
                var requestUri = "https://api.openai.com/v1/whisper/transcriptions"; 

                using var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(audioStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
                content.Add(streamContent, "file", "audio.wav");

                // Include language code if the API supports or requires it.
                // This may vary based on the actual API's capabilities and your requirements.
                content.Add(new StringContent(languageCode), "language");

                var response = await _httpClient.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                // The JSON parsing path here is hypothetical; adjust it based on the actual response structure.
                var transcript = json["transcription"].Value<string>();

                return transcript;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"HTTP request error: {e.Message}", e);
                throw new ApplicationException("An error occurred when calling the OpenAI transcription service.", e);
            }
            catch (Exception e)
            {
                _logger.LogError($"An unexpected error occurred: {e.Message}", e);
                throw new ApplicationException("An unexpected error occurred in WhisperService.", e);
            }
        }
    }
}
