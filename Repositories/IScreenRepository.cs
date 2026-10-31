using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Repositories.Interfaces;

public interface IScreenRepository
{
    Task<List<Screen>> GetAllAsync();
    Task<Screen?> GetByIdAsync(int id);
    Task AddAsync(Screen screen);
    Task UpdateAsync(Screen screen);
    Task DeleteAsync(Screen screen);
}