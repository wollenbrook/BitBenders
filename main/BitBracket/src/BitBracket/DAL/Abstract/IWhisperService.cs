namespace BitBracket.DAL.Abstract
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IWhisperService
    {
        Task<string> TranscribeAudioAsync(Stream audioStream, string languageCode);
    }
}
