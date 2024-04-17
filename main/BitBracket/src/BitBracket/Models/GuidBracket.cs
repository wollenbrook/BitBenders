using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

[Table("GuidBracket")]
public partial class GuidBracket
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(4000)]
    [Unicode(false)]
    public string BracketData { get; set; } = null!;
}
