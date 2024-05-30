using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

public partial class Friend
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("FriendID")]
    public int? FriendId { get; set; }

    [ForeignKey("FriendId")]
    [InverseProperty("FriendFriendNavigations")]
    public virtual BitUser? FriendNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("FriendUsers")]
    public virtual BitUser? User { get; set; }
}
