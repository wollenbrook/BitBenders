using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BitBracket.Models;

public class SentJoinRequest
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("PlayerSender")]
    public int SenderId { get; set; }

    [ForeignKey("Tournament")]
    public int TournamentId { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Pending";

    [InverseProperty("SentJoinRequests")]
    public virtual BitUser PlayerSender { get; set; }

    [JsonIgnore]
    [InverseProperty("SentJoinRequests")]
    public virtual Tournament Tournament { get; set; }
}
