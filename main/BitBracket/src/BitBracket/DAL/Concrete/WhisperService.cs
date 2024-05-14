//DAL/Concrete/WhisperService.CS

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using BitBracket.DAL.Abstract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

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
            _apiKey = configuration["BitBracketOpenAI2"]; // Make sure this is properly configured in your secrets.

            // Set up HttpClient instance with OpenAI API base URL and authentication headers.
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<(bool IsSuccess, string Text, string ErrorMessage)> TranscribeAudioAsync(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return (false, null, "No audio file provided or file is empty.");
            }

            try
            {
                var requestUri = "https://api.openai.com/v1/audio/transcriptions";
                using var content = new MultipartFormDataContent();
                using var audioStream = audioFile.OpenReadStream();
                var streamContent = new StreamContent(audioStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mp3");
                content.Add(streamContent, "file", audioFile.FileName);
                content.Add(new StringContent("whisper-1"), "model");

                var response = await _httpClient.PostAsync(requestUri, content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return (false, null, $"API error: {response.StatusCode} - {errorResponse}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);
                var text = json["text"].Value<string>();

                if (string.IsNullOrWhiteSpace(text))
                {
                    return (false, null, "Voice not clear, try again please.");
                }

                return (true, text, null);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"HTTP request error: {e.Message}", e);
                return (false, null, "An error occurred when calling the Whisper transcription service.");
            }
            catch (Exception e)
            {
                _logger.LogError($"An unexpected error occurred: {e.Message}", e);
                return (false, null, "An unexpected error occurred in the Whisper service.");
            }
        }
    }
}
