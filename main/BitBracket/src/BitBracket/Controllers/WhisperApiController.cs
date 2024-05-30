//Controllers/WhisperApiController.cs

using Microsoft.AspNetCore.Mvc;
using BitBracket.DAL.Abstract;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BitBracket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhisperApiController : ControllerBase
    {
        private readonly IWhisperService _whisperService;

        public WhisperApiController(IWhisperService whisperService)
        {
            _whisperService = whisperService;
        }

        [HttpPost("transcribe")]
        public async Task<IActionResult> TranscribeAudio(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("No audio file uploaded.");
            }

            var transcriptionResult = await _whisperService.TranscribeAudioAsync(audioFile);
            if (transcriptionResult.IsSuccess)
            {
                return Ok(transcriptionResult.Text);
            }
            return BadRequest(transcriptionResult.ErrorMessage);
        }
    }
}
