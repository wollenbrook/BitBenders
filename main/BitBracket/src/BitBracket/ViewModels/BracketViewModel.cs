using System.ComponentModel.DataAnnotations;

namespace BitBracket.ViewModels
{
    public class BracketViewModel
    {
        public string BracketName { get; set; } = null!;
        public int TournamentId { get; set; }
        public string BracketData { get; set; } = null!;
        public bool IsUserBracket { get; set; }
    }
}