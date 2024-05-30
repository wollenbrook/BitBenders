﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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

    [InverseProperty("BlockedUserNavigation")]
    public virtual ICollection<BlockedUser> BlockedUserBlockedUserNavigations { get; set; } = new List<BlockedUser>();

    [InverseProperty("Blocked")]
    public virtual ICollection<BlockedUser> BlockedUserBlockeds { get; set; } = new List<BlockedUser>();

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

    [InverseProperty("Sender")]
    public virtual ICollection<ParticipateRequest> ParticipateRequests { get; set; } = new List<ParticipateRequest>();

    [InverseProperty("User")]
    public virtual ICollection<Participate> Participates { get; set; } = new List<Participate>();

    [InverseProperty("PersonNavigation")]
    public virtual ICollection<Standing> Standings { get; set; } = new List<Standing>();

    [InverseProperty("OwnerNavigation")]
    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();

    [InverseProperty("BitUser")]
    public virtual ICollection<UserAnnouncement> UserAnnouncements { get; set; } = new List<UserAnnouncement>();
}
