using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

public partial class Announcement
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    [StringLength(500)]
    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    [StringLength(50)]
    public string Author { get; set; } = null!;

    public bool IsDraft { get; set; }
    
    public int? TournamentId { get; set; } // Nullable for announcements not tied to a specific tournament
    public int BitUserId { get; set; } // Direct link to the BitUser

        // Navigation properties
    public virtual BitUser BitUser { get; set; } = null!;
    public virtual Tournament? Tournament { get; set; }
}
