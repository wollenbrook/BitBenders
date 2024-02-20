using System;
using System.Collections.Generic;

namespace BitBracket.Models;

public partial class Announcement
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public string Author { get; set; } = null!;
}
//