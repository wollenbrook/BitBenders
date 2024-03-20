using System.ComponentModel.DataAnnotations;

namespace BitBracket.ViewModels
{
    public class TournamentViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(255, ErrorMessage = "Location cannot be longer than 255 characters.")]
        public string Location { get; set; } = null!;
    }
}