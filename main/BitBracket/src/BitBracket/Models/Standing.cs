using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

public partial class Standing
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int Placement { get; set; }

    [Column("TournamentID")]
    public int? TournamentId { get; set; }

    public int? Person { get; set; }

    [StringLength(4000)]
    public string Notes { get; set; } = "No current notes";

    [ForeignKey("Person")]
    [InverseProperty("Standings")]
    public virtual BitUser? PersonNavigation { get; set; }

    [ForeignKey("TournamentId")]
    [InverseProperty("Standings")]
    public virtual Tournament? Tournament { get; set; }
}
