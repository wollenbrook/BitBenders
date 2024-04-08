using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public DateTime Created { get; set; }

    public int? OwnerID { get; set; }

    [InverseProperty("Tournament")]
    public virtual ICollection<Bracket> Brackets { get; set; } = new List<Bracket>();

    [ForeignKey("Owner")]
    [InverseProperty("Tournaments")]
    public virtual BitUser? OwnerNavigation { get; set; }
}
