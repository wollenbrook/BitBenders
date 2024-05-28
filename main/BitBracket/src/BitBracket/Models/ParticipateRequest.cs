//Models/ParticipateRequest.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitBracket.Models
{
    public class ParticipateRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public virtual BitUser Sender { get; set; }

        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }

        public string? Status { get; set; } = null; // Pending, Approved, Rejected
    }
}
