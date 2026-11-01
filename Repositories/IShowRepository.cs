using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Repositories;

public interface IShowRepository
{
    Task<List<Show>> GetAllAsync();
    Task<Show?> GetByIdAsync(int id);
    Task<bool> MovieExistsAsync(int movieId);
    Task<bool> ScreenExistsAsync(int screenId);
    Task<Show> AddAsync(Show show);
    Task UpdateAsync(Show show);
    Task DeleteAsync(Show show);
}