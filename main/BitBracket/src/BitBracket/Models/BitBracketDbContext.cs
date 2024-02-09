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
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Announcement> Announcements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseLazyLoadingProxies()        // <-- add this line
                .UseSqlServer("Name=DatabaseConnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
