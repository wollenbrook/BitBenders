using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

public partial class FriendRequest
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("SenderID")]
    public int? SenderId { get; set; }

    [Column("ReceiverID")]
    public int? ReceiverId { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [ForeignKey("ReceiverId")]
    [InverseProperty("FriendRequestReceivers")]
    public virtual BitUser? Receiver { get; set; }

    [ForeignKey("SenderId")]
    [InverseProperty("FriendRequestSenders")]
    public virtual BitUser? Sender { get; set; }
}
