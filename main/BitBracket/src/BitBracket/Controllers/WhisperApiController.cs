using Microsoft.AspNetCore.Mvc;
using BitBracket.DAL.Abstract;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class WhisperApiController : ControllerBase
{
    private readonly IWhisperService _whisperService;

    public WhisperApiController(IWhisperService whisperService)
    {
        _whisperService = whisperService;
    }

    [HttpPost("ConvertSpeechToText")]
    public async Task<IActionResult> ConvertSpeechToText([FromForm] IFormFile audioData, [FromForm] string language)
    {
        if (audioData == null) return BadRequest("No audio data provided.");

        using (var memoryStream = new MemoryStream())
        {
            await audioData.CopyToAsync(memoryStream);
            var audioBytes = memoryStream.ToArray();
            var text = await _whisperService.ConvertSpeechToTextAsync(audioBytes, language);
            return Ok(text);
        }
    }
}
