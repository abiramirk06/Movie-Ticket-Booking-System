
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Database tables
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Theatre> Theatres => Set<Theatre>();
    public DbSet<Screen> Screens => Set<Screen>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Show> Shows => Set<Show>();
    public DbSet<ShowSeat> ShowSeats => Set<ShowSeat>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingSeat> BookingSeats => Set<BookingSeat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Screen - Theatre
        modelBuilder.Entity<Screen>()
            .HasOne(s => s.Theatre)
            .WithMany(t => t.Screens)
            .HasForeignKey(s => s.TheatreId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Screen>()
            .HasIndex(s => new { s.TheatreId, s.ScreenName })
            .IsUnique();

        // Seat - Screen
        modelBuilder.Entity<Seat>()
            .HasOne(s => s.Screen)
            .WithMany()
            .HasForeignKey(s => s.ScreenId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Seat>()
            .HasIndex(s => new { s.ScreenId, s.RowName, s.SeatNumber })
            .IsUnique();

        // Show - Movie
        modelBuilder.Entity<Show>()
            .HasOne(s => s.Movie)
            .WithMany()
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        // Show - Screen
        modelBuilder.Entity<Show>()
            .HasOne(s => s.Screen)
            .WithMany()
            .HasForeignKey(s => s.ScreenId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Show>()
            .Property(s => s.TicketPrice)
            .HasPrecision(10, 2);

        // ShowSeat - Show
        modelBuilder.Entity<ShowSeat>()
            .HasOne(ss => ss.Show)
            .WithMany(s => s.ShowSeats)
            .HasForeignKey(ss => ss.ShowId)
            .OnDelete(DeleteBehavior.Cascade);

        // ShowSeat - Seat
        modelBuilder.Entity<ShowSeat>()
            .HasOne(ss => ss.Seat)
            .WithMany()
            .HasForeignKey(ss => ss.SeatId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShowSeat>()
            .HasIndex(ss => new { ss.ShowId, ss.SeatId })
            .IsUnique();

        // Booking
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasIndex(b => b.BookingReference).IsUnique();

            entity.Property(b => b.TotalAmount)
                .HasPrecision(10, 2);



            entity.ToTable(t =>
                t.HasCheckConstraint(
                    "CK_Bookings_TotalAmount",
                    "[TotalAmount] >= 0"));

            entity.HasOne(b => b.Show)
                .WithMany()
                .HasForeignKey(b => b.ShowId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.User)
    .WithMany()
    .HasForeignKey(b => b.UserId)
    .OnDelete(DeleteBehavior.Restrict);
        });

        // BookingSeat
        modelBuilder.Entity<BookingSeat>(entity =>
        {
            entity.Property(bs => bs.Price)
                .HasPrecision(10, 2);

            entity.ToTable(t =>
                t.HasCheckConstraint(
                    "CK_BookingSeats_Price",
                    "[Price] >= 0"));

            entity.HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(bs => bs.Seat)
                .WithMany()
                .HasForeignKey(bs => bs.SeatId)
                .OnDelete(DeleteBehavior.Restrict);


        });
    }
}
