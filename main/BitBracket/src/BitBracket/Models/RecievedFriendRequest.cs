using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

public partial class RecievedFriendRequest
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("SenderID")]
    public int? SenderId { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [ForeignKey("SenderId")]
    [InverseProperty("RecievedFriendRequests")]
    public virtual BitUser? Sender { get; set; }
}
