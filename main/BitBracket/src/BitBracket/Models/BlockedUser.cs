using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

public partial class BlockedUser
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BlockedID")]
    public int? BlockedId { get; set; }

    [Column("BlockedUserID")]
    public int? BlockedUserId { get; set; }


    [ForeignKey("BlockedId")]
    [InverseProperty("BlockedUserBlockeds")]
    public virtual BitUser? Blocked { get; set; }

    [ForeignKey("BlockedUserId")]
    [InverseProperty("BlockedUserBlockedUserNavigations")]
    public virtual BitUser? BlockedUserNavigation { get; set; }
}
