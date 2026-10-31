using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Theatre> Theatres => Set<Theatre>();
    public DbSet<Screen> Screens => Set<Screen>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Screen>()
            .HasOne(s => s.Theatre)
            .WithMany(t => t.Screens)
            .HasForeignKey(s => s.TheatreId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Screen>()
            .HasIndex(s => new { s.TheatreId, s.ScreenName })
            .IsUnique();
    }
}
