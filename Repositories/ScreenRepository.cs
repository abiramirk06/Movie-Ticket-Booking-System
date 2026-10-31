using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.API.Data;
using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Repositories;

public class ScreenRepository : IScreenRepository
{
    private readonly AppDbContext _context;

    public ScreenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Screen>> GetAllAsync()
    {
        return await _context.Screens
            .Include(s => s.Theatre)
            .ToListAsync();
    }

    public async Task<Screen?> GetByIdAsync(int id)
    {
        return await _context.Screens
            .Include(s => s.Theatre)
            .FirstOrDefaultAsync(s => s.ScreenId == id);
    }

    public async Task AddAsync(Screen screen)
    {
        await _context.Screens.AddAsync(screen);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Screen screen)
    {
        _context.Screens.Update(screen);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Screen screen)
    {
        _context.Screens.Remove(screen);
        await _context.SaveChangesAsync();
    }
}