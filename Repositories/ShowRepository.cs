using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.API.Data;
using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Repositories;

public class ShowRepository : IShowRepository
{
    private readonly AppDbContext _context;

    public ShowRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Show>> GetAllAsync()
    {
        return await _context.Shows.ToListAsync();
    }

    public async Task<Show?> GetByIdAsync(int id)
    {
        return await _context.Shows
            .FirstOrDefaultAsync(s => s.ShowId == id);
    }

    public async Task<bool> MovieExistsAsync(int movieId)
    {
        return await _context.Movies
            .AnyAsync(m => m.MovieId == movieId);
    }

    public async Task<bool> ScreenExistsAsync(int screenId)
    {
        return await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);
    }

    public async Task<Show> AddAsync(Show show)
    {
        using var transaction =
            await _context.Database.BeginTransactionAsync();

        // Create the show
        await _context.Shows.AddAsync(show);
        await _context.SaveChangesAsync();

        // Get all seats belonging to this screen
        var seats = await _context.Seats
            .Where(s => s.ScreenId == show.ScreenId)
            .ToListAsync();

        if (seats.Count == 0)
        {
            throw new InvalidOperationException(
                "No seats found for this screen.");
        }

        // Automatically create ShowSeats
        var showSeats = seats.Select(seat => new ShowSeat
        {
            ShowId = show.ShowId,
            SeatId = seat.SeatId,
            Status = "Available"
        }).ToList();

        await _context.ShowSeats.AddRangeAsync(showSeats);
        await _context.SaveChangesAsync();

        // Commit both operations together
        await transaction.CommitAsync();

        return show;
    }

    public async Task UpdateAsync(Show show)
    {
        _context.Shows.Update(show);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Show show)
    {
        _context.Shows.Remove(show);
        await _context.SaveChangesAsync();
    }
}