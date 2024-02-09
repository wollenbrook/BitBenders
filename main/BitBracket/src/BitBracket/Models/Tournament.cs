using System;

namespace BitBracket.Models
{
    public class Tournament
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public string? Brackets { get; set; }
        public DateTime Created { get; set; }
    }
}
