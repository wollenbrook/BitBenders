using System;

namespace BitBracket.Models
{
    public class Announcement
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Author { get; set; }
    }
}
