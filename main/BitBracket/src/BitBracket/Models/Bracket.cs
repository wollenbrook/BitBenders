using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

public partial class Bracket
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [StringLength(4000)]
    public string BracketData { get; set; } = null!;

    [Column("TournamentID")]
    public int? TournamentID { get; set; }

    [ForeignKey("TournamentId")]
    [InverseProperty("Brackets")]
    public virtual Tournament? Tournament { get; set; }
}
