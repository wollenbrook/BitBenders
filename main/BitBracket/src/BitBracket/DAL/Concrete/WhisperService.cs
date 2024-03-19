using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BitBracket.DAL.Abstract;
using Newtonsoft.Json;

namespace BitBracket.DAL.Concrete
{
    public class WhisperService : IWhisperService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "BitBracketOpenAI"; // Ensure this is securely stored and accessed
        private const string WhisperApiUrl = "https://api.openai.com/v1/whisper"; // Confirm the correct API URL

        public WhisperService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        }

        public async Task<string> ConvertSpeechToTextAsync(byte[] audioData, string language)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new ByteArrayContent(audioData), "audio", "audio.wav");
                    content.Add(new StringContent(language), "language"); // Confirm if the API supports this parameter

                    var response = await _httpClient.PostAsync(WhisperApiUrl, content);
                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<WhisperResponse>(responseContent);

                    return result.Text;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error processing speech to text", ex);
            }
        }

        private class WhisperResponse
        {
            public string Text { get; set; }
        }
    }
}
