using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using BitBracket.DAL.Concrete;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;

namespace BitBracket_NUnit_Tests
{
    public class WhisperServiceTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private WhisperService _whisperService;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        
        [SetUp]
        public void SetUp()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            
            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("https://api.openai.com/")
            };

            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            
            // Setup IConfiguration to return API key
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["BitBracketOpenAI"]).Returns("YourAPIKeyHere");
            
            _whisperService = new WhisperService(_mockHttpClientFactory.Object, mockConfiguration.Object, new NullLogger<WhisperService>());
            
            // Mock the response
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"transcription\":\"This is a test transcription.\"}")
                });
        }

        [Test]
        public async Task TranscribeAudioAsync_ShouldReturnCorrectTranscription()
        {
            var result = await _whisperService.TranscribeAudioAsync(new MemoryStream(Encoding.UTF8.GetBytes("dummy audio data")), "en");
            
            Assert.AreEqual("This is a test transcription.", result);
        }

        [Test]
        public async Task TranscribeAudioAsync_ShouldHandleEmptyTranscriptionResponse()
        {
            // Arrange: Simulate API returning a successful response with an empty transcription
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"transcription\":\"\"}")
                });

            // Act
            var result = await _whisperService.TranscribeAudioAsync(new MemoryStream(Encoding.UTF8.GetBytes("dummy audio data")), "en");

            // Assert: Verify that an empty transcription is handled gracefully
            Assert.IsEmpty(result);
        }
    }
}
