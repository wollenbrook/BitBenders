using System;

namespace BitBracket.Models
{
    public class Announcement
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
    }
}
