using System.ComponentModel.DataAnnotations;

namespace BitBracket.ViewModels
{
    public class BroadcastLinkViewModel
    {
        public int TournamentId { get; set; }

        [Required(ErrorMessage = "Broadcast type is required.")]
        public string BroadcastType { get; set; } = null!;

        [Required(ErrorMessage = "Name or ID is required.")]
        [StringLength(255, ErrorMessage = "Name or ID cannot be longer than 255 characters.")]
        public string NameOrID { get; set; } = null!;
    }
}