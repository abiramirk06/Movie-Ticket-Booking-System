using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.API.Data;
using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Repositories.Interfaces;

namespace MovieTicketBookingSystem.API.Repositories;

public class TheatreRepository : ITheatreRepository
{
    private readonly AppDbContext _context;

    public TheatreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Theatre>> GetAllAsync()
    {
        return await _context.Theatres
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Theatre?> GetByIdAsync(int id)
    {
        return await _context.Theatres
            .FirstOrDefaultAsync(t => t.TheatreId == id);
    }

    public async Task<Theatre> AddAsync(Theatre theatre)
    {
        await _context.Theatres.AddAsync(theatre);
        await _context.SaveChangesAsync();

        return theatre;
    }

    public async Task UpdateAsync(Theatre theatre)
    {
        _context.Theatres.Update(theatre);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Theatre theatre)
    {
        _context.Theatres.Remove(theatre);
        await _context.SaveChangesAsync();
    }
}