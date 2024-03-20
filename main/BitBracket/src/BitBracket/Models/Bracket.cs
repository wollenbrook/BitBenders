using System;
using System.Collections.Generic;

namespace BitBracket.Models;

public partial class Bracket
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string BracketData { get; set; } = null!;

    public int? TournamentId { get; set; }

    public virtual Tournament1? Tournament { get; set; }
}
