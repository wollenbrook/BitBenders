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

    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }
    [Required]
    [StringLength(50)]
    public string? Author { get; set; }
}
