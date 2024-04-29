using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
    public string Bio { get; set; } = "bio not written";

    public byte[]? ProfilePicture { get; set; } = null;

    public bool EmailConfirmedStatus { get; set; } = false;
    public bool OptInConfirmation { get; set; } = true;

    [InverseProperty("FriendNavigation")]
    [JsonIgnore]
    public virtual ICollection<Friend> FriendFriendNavigations { get; set; } = new List<Friend>();


    [InverseProperty("Receiver")]
    [JsonIgnore]
    public virtual ICollection<FriendRequest> FriendRequestReceivers { get; set; } = new List<FriendRequest>();

    [InverseProperty("Sender")]
    [JsonIgnore]
    public virtual ICollection<FriendRequest> FriendRequestSenders { get; set; } = new List<FriendRequest>();

    [InverseProperty("User")]
    [JsonIgnore]
    public virtual ICollection<Friend> FriendUsers { get; set; } = new List<Friend>();

    [InverseProperty("OwnerNavigation")]
    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    
    [InverseProperty("BitUser")]
    public virtual ICollection<UserAnnouncement> UserAnnouncements { get; set; } = new List<UserAnnouncement>();

    [InverseProperty("Player")]
    public virtual ICollection<JoinedPlayer> JoinedTournaments { get; set; } = new List<JoinedPlayer>();

    [InverseProperty("PlayerSender")]
    public virtual ICollection<SentJoinRequest> SentJoinRequests { get; set; } = new List<SentJoinRequest>();

    [InverseProperty("OwnerReceiver")]
    public virtual ICollection<ReceivedPlayerRequest> ReceivedPlayerRequests { get; set; } = new List<ReceivedPlayerRequest>();
}
