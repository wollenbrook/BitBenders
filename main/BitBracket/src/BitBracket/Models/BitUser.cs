using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

[Table("BitUser")]
public partial class BitUser
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ASPNetIdentityID")]
    [StringLength(50)]
    public string AspnetIdentityId { get; set; } = null!;

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(50)]
    public string Tag { get; set; } = null!;

    [StringLength(500)]
    public string Bio { get; set; } = null!;

    public byte[]? ProfilePicture { get; set; }

    public bool EmailConfirmedStatus { get; set; }

    [InverseProperty("FriendNavigation")]
    public virtual ICollection<Friend> FriendFriendNavigations { get; set; } = new List<Friend>();

    [InverseProperty("User")]
    public virtual ICollection<Friend> FriendUsers { get; set; } = new List<Friend>();

    [InverseProperty("Sender")]
    public virtual ICollection<RecievedFriendRequest> RecievedFriendRequests { get; set; } = new List<RecievedFriendRequest>();

    [InverseProperty("Receiver")]
    public virtual ICollection<SentFriendRequest> SentFriendRequestReceivers { get; set; } = new List<SentFriendRequest>();

    [InverseProperty("Sender")]
    public virtual ICollection<SentFriendRequest> SentFriendRequestSenders { get; set; } = new List<SentFriendRequest>();

    [InverseProperty("OwnerNavigation")]
    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
