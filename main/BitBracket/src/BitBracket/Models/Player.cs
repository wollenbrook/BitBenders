using System;

namespace BitBracket.Models
{
    public class Player
    {
        public int PlayerID { get; set; }
        public Guid BracketID { get; set; }
        public string? PlayerTag { get; set; }
        public int PlayerSeed { get; set; }
    }
}