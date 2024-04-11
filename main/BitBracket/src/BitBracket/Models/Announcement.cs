using System;
using System.Collections.Generic;

namespace BitBracket.Models;

public partial class Announcement
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public string Author { get; set; } = null!;

    public bool IsDraft { get; set; }
    
    public int? TournamentId { get; set; } // Nullable for announcements not tied to a specific tournament
    public int BitUserId { get; set; } // Direct link to the BitUser

        // Navigation properties
    public virtual BitUser BitUser { get; set; } = null!;
    public virtual Tournament? Tournament { get; set; }
}
