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

    public virtual DbSet<BlockedUser> BlockedUsers { get; set; }

    public virtual DbSet<Bracket> Brackets { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<FriendRequest> FriendRequests { get; set; }

    public virtual DbSet<GuidBracket> GuidBrackets { get; set; }

    public virtual DbSet<Participate> Participates { get; set; }

    public virtual DbSet<ParticipateRequest> ParticipateRequests { get; set; }

    public virtual DbSet<Standing> Standings { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    public virtual DbSet<UserAnnouncement> UserAnnouncements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=BitBracketConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Announce__3214EC27A939F1BF");
        });

        modelBuilder.Entity<BitUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BitUser__3214EC27A5C9F082");
        });

        modelBuilder.Entity<BlockedUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlockedU__3214EC27BD564830");

            entity.HasOne(d => d.Blocked).WithMany(p => p.BlockedUserBlockeds).HasConstraintName("FK__BlockedUs__Block__54CB950F");

            entity.HasOne(d => d.BlockedUserNavigation).WithMany(p => p.BlockedUserBlockedUserNavigations).HasConstraintName("FK__BlockedUs__Block__55BFB948");
        });

        modelBuilder.Entity<Bracket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brackets__3214EC274E7A2F9B");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Brackets).HasConstraintName("FK__Brackets__Tourna__1209AD79");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friends__3214EC279B931AA5");

            entity.HasOne(d => d.FriendNavigation).WithMany(p => p.FriendFriendNavigations).HasConstraintName("FK__Friends__FriendI__1B9317B3");

            entity.HasOne(d => d.User).WithMany(p => p.FriendUsers).HasConstraintName("FK__Friends__UserID__1A9EF37A");
        });

        modelBuilder.Entity<FriendRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FriendRe__3214EC27EC3B4458");

            entity.HasOne(d => d.Receiver).WithMany(p => p.FriendRequestReceivers).HasConstraintName("FK__FriendReq__Recei__17C286CF");

            entity.HasOne(d => d.Sender).WithMany(p => p.FriendRequestSenders).HasConstraintName("FK__FriendReq__Sende__16CE6296");
        });

        modelBuilder.Entity<GuidBracket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GuidBrac__3214EC274939B1BE");
        });

        modelBuilder.Entity<Participate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Particip__3214EC277642FEF1");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Participates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Participa__Tourn__2F9A1060");

            entity.HasOne(d => d.User).WithMany(p => p.Participates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Participa__UserI__2EA5EC27");
        });

        modelBuilder.Entity<ParticipateRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Particip__3214EC27344E0881");

            entity.HasOne(d => d.Sender).WithMany(p => p.ParticipateRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Participa__Sende__32767D0B");

            entity.HasOne(d => d.Tournament).WithMany(p => p.ParticipateRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Participa__Tourn__336AA144");
        });

        modelBuilder.Entity<Standing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Standing__3214EC27B8BECD0C");

            entity.HasOne(d => d.PersonNavigation).WithMany(p => p.Standings).HasConstraintName("FK__Standings__Perso__4589517F");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Standings).HasConstraintName("FK__Standings__Tourn__44952D46");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3214EC27612B0B27");

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Tournaments).HasConstraintName("FK__Tournamen__Owner__0F2D40CE");
        });

        modelBuilder.Entity<UserAnnouncement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAnno__3214EC27200289F3");

            entity.HasOne(d => d.BitUser).WithMany(p => p.UserAnnouncements).HasConstraintName("FK__UserAnnou__Owner__1F63A897");

            entity.HasOne(d => d.Tournament).WithMany(p => p.TournamentID).HasConstraintName("FK__UserAnnou__Tourn__2057CCD0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
