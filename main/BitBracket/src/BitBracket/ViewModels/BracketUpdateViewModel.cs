using System.ComponentModel.DataAnnotations;

namespace BitBracket.ViewModels
{
    public class BracketUpdateViewModel
    {
        public int BracketId { get; set; }
        public string BracketData { get; set; } = null!;
    }
}