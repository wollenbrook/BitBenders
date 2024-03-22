//DAL/Abstract/IWhisperService.CS

using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface IWhisperService
    {
        Task<string> ConvertSpeechToTextAsync(byte[] audioData, string language);
    }
}
