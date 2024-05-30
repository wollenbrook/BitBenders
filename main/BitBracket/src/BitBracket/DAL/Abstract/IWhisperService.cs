//DAL/Abstract/IWhisperService.CS

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BitBracket.DAL.Abstract
{
    public interface IWhisperService
    {
        Task<(bool IsSuccess, string Text, string ErrorMessage)> TranscribeAudioAsync(IFormFile audioFile);
    }
}
