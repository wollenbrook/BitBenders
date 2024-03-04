using System.ComponentModel.DataAnnotations;

namespace BitBracket.ViewModels
{
    public class PlayerViewModel
    {
        [Required(ErrorMessage = "Player names are required.")]
        public string? Names { get; set; }
        public bool RandomSeeding { get; set; }
    }
}