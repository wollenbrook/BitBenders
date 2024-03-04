using System;
using System.Collections.Generic;

namespace BitBracket.Models;

public partial class Tournament
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Brackets { get; set; } = null!;

    public DateTime Created { get; set; } = DateTime.Now;
}
