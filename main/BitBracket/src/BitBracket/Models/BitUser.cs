using System;
using System.Collections.Generic;

namespace BitBracket.Models;

public partial class BitUser
{
    public int Id { get; set; }

    public string AspnetIdentityId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Tag { get; set; } = null!;

    public string Bio { get; set; } = null!;

    public byte[]? ProfilePicture { get; set; } = null!;

    public bool EmailConfirmedStatus { get; set; }

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
