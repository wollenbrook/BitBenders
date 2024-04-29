using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BitBracket.Models;

public class JoinedPlayer
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Player")]
    public int PlayerId { get; set; }

    [ForeignKey("Tournament")]
    public int TournamentId { get; set; }

    [InverseProperty("JoinedTournaments")]
    public virtual BitUser Player { get; set; }

    [InverseProperty("Players")]
    public virtual Tournament Tournament { get; set; }
}
