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
    public virtual DbSet<UserAnnouncement> UserAnnouncements { get; set; }

    public virtual DbSet<BitUser> BitUsers { get; set; }

    public virtual DbSet<Bracket> Brackets { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<GuidBracket> GuidBrackets { get; set; }

    public virtual DbSet<FriendRequest> FriendRequests { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }
    public virtual DbSet<Participate> Participates { get; set; } // Added for participation tracking
    public virtual DbSet<ParticipateRequest> ParticipateRequests { get; set; } // Added for participation requests

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=BitBracketConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Announce__3214EC27DBB7EADA");
        });

        modelBuilder.Entity<UserAnnouncement>(entity =>
        {
            //entity.ToTable("UserAnnouncements");
            entity.HasKey(e => e.Id).HasName("PK__UserAnno__3214EC27BF062C3D");
            entity.HasOne(d => d.BitUser).WithMany(p => p.UserAnnouncements).HasForeignKey(d => d.Owner).HasConstraintName("FK__UserAnnou__Owner__40058253").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.Tournament).WithMany(p => p.TournamentID).HasForeignKey(d => d.TournamentId).HasConstraintName("FK__UserAnnou__Tourn__40F9A68C").OnDelete(DeleteBehavior.SetNull);
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


        modelBuilder.Entity<FriendRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FriendRe__3214EC27340430CC");

            entity.HasOne(d => d.Receiver).WithMany(p => p.FriendRequestReceivers).HasConstraintName("FK__FriendReq__Recei__51300E55");

            entity.HasOne(d => d.Sender).WithMany(p => p.FriendRequestSenders).HasConstraintName("FK__FriendReq__Sende__503BEA1C");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3214EC274F52B401");

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Tournaments).HasConstraintName("FK__Tournamen__Owner__71D1E811");
        });

        modelBuilder.Entity<Participate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Participate");
            entity.HasOne(d => d.User).WithMany(p => p.Participates).HasForeignKey(d => d.UserId);
            entity.HasOne(d => d.Tournament).WithMany(p => p.Participates).HasForeignKey(d => d.TournamentId);
        });

        modelBuilder.Entity<ParticipateRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ParticipateRequest");
            entity.HasOne(d => d.Sender).WithMany(p => p.ParticipateRequestSenders).HasForeignKey(d => d.SenderId);
            entity.HasOne(d => d.Tournament).WithMany(p => p.ParticipateRequests).HasForeignKey(d => d.TournamentId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
