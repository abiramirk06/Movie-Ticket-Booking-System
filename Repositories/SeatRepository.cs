using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.API.Data;
using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly AppDbContext _context;

    public SeatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Seat>> GetAllAsync()
    {
        return await _context.Seats.ToListAsync();
    }

    public async Task<Seat?> GetByIdAsync(int id)
    {
        return await _context.Seats
            .FirstOrDefaultAsync(s => s.SeatId == id);
    }

    public async Task AddAsync(Seat seat)
    {
        await _context.Seats.AddAsync(seat);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Seat seat)
    {
        _context.Seats.Update(seat);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Seat seat)
    {
        _context.Seats.Remove(seat);
        await _context.SaveChangesAsync();
    }
}