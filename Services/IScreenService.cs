using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Services;

public interface IScreenService
{
    Task<List<Screen>> GetAllScreensAsync();

    Task<Screen?> GetScreenByIdAsync(int id);

    Task<Screen> CreateScreenAsync(Screen screen);

    Task<bool> UpdateScreenAsync(int id, Screen screen);

    Task<bool> DeleteScreenAsync(int id);
}