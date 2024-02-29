using System;
using System.Collections.Generic;

namespace BitBracket.Models;

public partial class BitUser
{
    public int Id { get; set; }

    public string AspnetIdentityId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Tag { get; set; } = null!;

    public string Bio { get; set; } = "Bio not written";

    public byte[]? ProfilePicture { get; set; } = File.ReadAllBytes("Images/Blank_Profile.png");
}
