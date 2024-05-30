using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BitBracket.Controllers;
using BitBracket.DAL.Abstract;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace BitBracket.Tests
{
    public class WhisperApiControllerTests
    {
        private Mock<IWhisperService> _mockWhisperService; // Declare a mock object for IWhisperService.
        private WhisperApiController _controller; // Declare an instance of the controller to be tested.

        [SetUp]
        public void Setup()
        {
            _mockWhisperService = new Mock<IWhisperService>(); // Initialize the mock object.
            _controller = new WhisperApiController(_mockWhisperService.Object); // Initialize the controller with the mock object.
        }

        [Test]
        public async Task TranscribeAudio_NoAudioFile_ReturnsBadRequest()
        {
            // Arrange
            IFormFile audioFile = null; // Simulate no audio file being uploaded.

            // Act
            var result = await _controller.TranscribeAudio(audioFile); // Call the controller action.

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result); // Check if the result is a BadRequestObjectResult.
            var badRequestResult = result as BadRequestObjectResult; // Cast the result to BadRequestObjectResult.
            Assert.AreEqual("No audio file uploaded.", badRequestResult.Value); // Verify the error message.
        }

        [Test]
        public async Task TranscribeAudio_SuccessfulTranscription_ReturnsOk()
        {
            // Arrange
            var audioFileMock = new Mock<IFormFile>(); // Create a mock object for IFormFile.
            audioFileMock.Setup(f => f.Length).Returns(100); // Mock the Length property.
            var fakeStream = new MemoryStream(); // Create a fake memory stream.
            audioFileMock.Setup(f => f.OpenReadStream()).Returns(fakeStream); // Mock the OpenReadStream method.
            audioFileMock.Setup(f => f.FileName).Returns("test.mp3"); // Mock the FileName property.

            // Mock the service method to return a successful transcription.
            _mockWhisperService.Setup(s => s.TranscribeAudioAsync(audioFileMock.Object))
                .ReturnsAsync((true, "Transcription text", null));

            // Act
            var result = await _controller.TranscribeAudio(audioFileMock.Object); // Call the controller action.

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result); // Check if the result is an OkObjectResult.
            var okResult = result as OkObjectResult; // Cast the result to OkObjectResult.
            Assert.AreEqual("Transcription text", okResult.Value); // Verify the transcription text.
        }

        [Test]
        public async Task TranscribeAudio_UnsuccessfulTranscription_ReturnsBadRequest()
        {
            // Arrange
            var audioFileMock = new Mock<IFormFile>(); // Create a mock object for IFormFile.
            audioFileMock.Setup(f => f.Length).Returns(100); // Mock the Length property.
            var fakeStream = new MemoryStream(); // Create a fake memory stream.
            audioFileMock.Setup(f => f.OpenReadStream()).Returns(fakeStream); // Mock the OpenReadStream method.
            audioFileMock.Setup(f => f.FileName).Returns("test.mp3"); // Mock the FileName property.

            // Mock the service method to return an unsuccessful transcription.
            _mockWhisperService.Setup(s => s.TranscribeAudioAsync(audioFileMock.Object))
                .ReturnsAsync((false, null, "Transcription failed"));

            // Act
            var result = await _controller.TranscribeAudio(audioFileMock.Object); // Call the controller action.

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result); // Check if the result is a BadRequestObjectResult.
            var badRequestResult = result as BadRequestObjectResult; // Cast the result to BadRequestObjectResult.
            Assert.AreEqual("Transcription failed", badRequestResult.Value); // Verify the error message.
        }
    }
}