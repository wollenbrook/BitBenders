using System;
using System.Collections.Generic;
using BitBracket.Models;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.Data;

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

    public virtual DbSet<Tournament> Tournaments { get; set; }

    public virtual DbSet<Tournament1> Tournaments1 { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=BitBracketConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Announce__3214EC27BF9A6C49");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Author).HasMaxLength(50);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<BitUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BitUser__3214EC2764D6580A");

            entity.ToTable("BitUser");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AspnetIdentityId)
                .HasMaxLength(50)
                .HasColumnName("ASPNetIdentityID");
            entity.Property(e => e.Bio).HasMaxLength(500);
            entity.Property(e => e.Tag).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Bracket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brackets__3214EC27FB3F90CE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BracketData).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TournamentId).HasColumnName("TournamentID");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Brackets)
                .HasForeignKey(d => d.TournamentId)
                .HasConstraintName("FK__Brackets__Tourna__1CBC4616");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3214EC275C230857");

            entity.ToTable("Tournament");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Brackets).HasMaxLength(50);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Tournament1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3214EC2739091D84");

            entity.ToTable("Tournaments");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Tournament1s)
                .HasForeignKey(d => d.Owner)
                .HasConstraintName("FK__Tournamen__Owner__19DFD96B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
