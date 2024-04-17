using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Models;

public partial class BitBracketDbContext : DbContext
{
    public BitBracketDbContext()
    {
    }

    public BitBracketDbContext(DbContextOptions<BitBracketDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<BitUser> BitUsers { get; set; }

    public virtual DbSet<Bracket> Brackets { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<GuidBracket> GuidBrackets { get; set; }

    public virtual DbSet<RecievedFriendRequest> RecievedFriendRequests { get; set; }

    public virtual DbSet<SentFriendRequest> SentFriendRequests { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=BitBracketConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Announce__3214EC27DBB7EADA");
        });

        modelBuilder.Entity<BitUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BitUser__3214EC27D98320F7");
        });

        modelBuilder.Entity<Bracket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brackets__3214EC27B117FB09");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Brackets).HasConstraintName("FK__Brackets__Tourna__74AE54BC");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friends__3214EC271CEE40C2");

            entity.HasOne(d => d.FriendNavigation).WithMany(p => p.FriendFriendNavigations).HasConstraintName("FK__Friends__FriendI__01142BA1");

            entity.HasOne(d => d.User).WithMany(p => p.FriendUsers).HasConstraintName("FK__Friends__UserID__00200768");
        });

        modelBuilder.Entity<GuidBracket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GuidBrac__3214EC27673B9F11");
        });

        modelBuilder.Entity<RecievedFriendRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recieved__3214EC27280873E1");

            entity.HasOne(d => d.Sender).WithMany(p => p.RecievedFriendRequests).HasConstraintName("FK__RecievedF__Sende__7D439ABD");
        });

        modelBuilder.Entity<SentFriendRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SentFrie__3214EC2750936D48");

            entity.HasOne(d => d.Receiver).WithMany(p => p.SentFriendRequestReceivers).HasConstraintName("FK__SentFrien__Recei__7A672E12");

            entity.HasOne(d => d.Sender).WithMany(p => p.SentFriendRequestSenders).HasConstraintName("FK__SentFrien__Sende__797309D9");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3214EC274F52B401");

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Tournaments).HasConstraintName("FK__Tournamen__Owner__71D1E811");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
