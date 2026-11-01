using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Services;

public interface IShowService
{
    Task<List<Show>> GetAllShowsAsync();
    Task<Show?> GetShowByIdAsync(int id);
    Task<Show> CreateShowAsync(Show show);
    Task<bool> UpdateShowAsync(int id, Show show);
    Task<bool> DeleteShowAsync(int id);
}