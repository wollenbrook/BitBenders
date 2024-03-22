using System;
using System.Collections.Generic;

namespace BitBracket.Models;

public partial class Tournament1
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime Created { get; set; }

    public int? Owner { get; set; }

    public virtual ICollection<Bracket> Brackets { get; set; } = new List<Bracket>();

    public virtual BitUser? OwnerNavigation { get; set; }
}
