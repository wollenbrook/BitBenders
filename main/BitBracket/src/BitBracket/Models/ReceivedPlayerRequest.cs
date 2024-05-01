using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BitBracket.Models;

public class ReceivedPlayerRequest
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("OwnerReceiver")]
    public int ReceiverId { get; set; }

    [ForeignKey("Tournament")]
    public int TournamentId { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Pending";

    [InverseProperty("ReceivedPlayerRequests")]
    public virtual BitUser OwnerReceiver { get; set; }

    [JsonIgnore]
    [InverseProperty("ReceivedPlayerRequests")]
    public virtual Tournament Tournament { get; set; }
}
