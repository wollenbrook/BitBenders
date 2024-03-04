using System.ComponentModel.DataAnnotations;

namespace BitBracket.ViewModels
{
    public class BasicBracketViewModel
    {
        [Required(ErrorMessage = "Player names are required.")]
        public string? Names { get; set; }
        public string? Format { get; set; }
        public bool RandomSeeding { get; set; }
    }
}