using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BitBracket.Models;

public partial class UserAnnouncement
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string? Title { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsDraft { get; set; } = true;  // Default is true for drafts

    [Required]
    [StringLength(50)]
    public string? Author { get; set; } = null!;

    public int? Owner { get; set; }

    [Column("TournamentID")]
    public int? TournamentId { get; set; }

    [ForeignKey("Owner")]
    [JsonIgnore]
    [InverseProperty("UserAnnouncements")]
    public virtual BitUser? BitUser { get; set; }

    [ForeignKey("TournamentId")]
    [JsonIgnore]
    [InverseProperty("TournamentID")]
    public virtual Tournament? Tournament { get; set; }
}
