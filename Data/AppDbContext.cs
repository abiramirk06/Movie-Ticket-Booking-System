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
}