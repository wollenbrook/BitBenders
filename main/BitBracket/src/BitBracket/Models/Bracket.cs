using System;

namespace BitBracket.Models
{
    public class Bracket
    {
        public int ID { get; set; }

        public string Name { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string BracketData { get; set; } = null!;

        public int TournamentID { get; set; }
    }
}