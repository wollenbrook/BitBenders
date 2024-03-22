using NUnit.Framework;
using BitBracket.DAL.Concrete;
using System.Threading.Tasks;
using Moq;
using System.Net.Http;
using System;
using BitBracket.DAL.Abstract;
using Moq.Protected;
using System.Net.Http.Headers;

namespace BitBracket_NUnit_Tests
{
    public class WhisperServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private WhisperService _whisperService;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private const string ApiUrl = "https://api.openai.com/v1/whisper";

        [SetUp]
        public void Setup()
        {
            // Create a new Mock of the HttpMessageHandler
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Setup the mock to handle a SendAsync request
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{\"Text\":\"This is a test transcription.\"}"),
                })
                .Verifiable();

            // Create a mock HttpClientFactory
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(ApiUrl),
            };

            // Setup the factory to return the mock HttpClient
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Initialize your service with the mock IHttpClientFactory
            _whisperService = new WhisperService(_mockHttpClientFactory.Object);
        }

        [Test]
        public async Task ConvertSpeechToTextAsync_WhenCalled_ReturnsTranscribedText()
        {
            var audioData = new byte[] { 1, 2, 3 }; // Example audio data
            var language = "en"; // Example language code

            var result = await _whisperService.ConvertSpeechToTextAsync(audioData, language);

            Assert.AreEqual("This is a test transcription.", result);
            
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri.ToString() == ApiUrl
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
