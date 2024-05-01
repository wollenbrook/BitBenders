//Models/Tournament.cs

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

public partial class Tournament
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string Location { get; set; } = null!;

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Created { get; set; } = DateTime.Now;

    public int? Owner { get; set; }

    [InverseProperty("Tournament")]
    public virtual ICollection<Bracket> Brackets { get; set; } = new List<Bracket>();

    [ForeignKey("Owner")]
    [JsonIgnore]
    [InverseProperty("Tournaments")]
    public virtual BitUser? OwnerNavigation { get; set; }

    [InverseProperty("Tournament")]
    public virtual ICollection<UserAnnouncement> TournamentID { get; set; } = new List<UserAnnouncement>();  // Navigation property for Announcements

    // New additions for participation
    [InverseProperty("Tournament")]
    public virtual ICollection<Participate> Participates { get; set; } = new List<Participate>();

    [InverseProperty("Tournament")]
    public virtual ICollection<ParticipateRequest> ParticipateRequests { get; set; } = new List<ParticipateRequest>();

}
