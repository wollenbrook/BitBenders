using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BitBracket.Models;

[Table("UserAnnouncements")]
public partial class UserAnnouncement
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [StringLength(50)]
    public string? Author { get; set; }

    public bool IsDraft { get; set; } = true;  // Default is true for drafts

    public int? Owner { get; set; }
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
