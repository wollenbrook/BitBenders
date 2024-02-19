using System;
using System.ComponentModel.DataAnnotations;

namespace BitBracket.Models
{
    public class Announcement
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "The title is required.")]
        [StringLength(50, ErrorMessage = "The title must be less than 50 characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The creation date is required.")]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "The description is required.")]
        [StringLength(500, ErrorMessage = "The description must be less than 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The status indicating if the announcement is active is required.")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "The author's name is required.")]
        [StringLength(50, ErrorMessage = "The author's name must be less than 50 characters.")]
        public string? Author { get; set; }
    }
}
