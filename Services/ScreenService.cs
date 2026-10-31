using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Repositories;

namespace MovieTicketBookingSystem.API.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository _repository;

    public ScreenService(IScreenRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Screen>> GetAllScreensAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Screen?> GetScreenByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid screen ID.");

        return await _repository.GetByIdAsync(id);
    }

    public async Task<Screen> CreateScreenAsync(Screen screen)
    {
        if (screen.TheatreId <= 0)
            throw new ArgumentException("Valid TheatreId is required.");

        if (string.IsNullOrWhiteSpace(screen.ScreenName))
            throw new ArgumentException("Screen name is required.");

        screen.ScreenName = screen.ScreenName.Trim();

        await _repository.AddAsync(screen);

        return screen;
    }

    public async Task<bool> UpdateScreenAsync(int id, Screen screen)
    {
        if (id <= 0 || screen.TheatreId <= 0)
            throw new ArgumentException("Invalid screen or theatre ID.");

        if (string.IsNullOrWhiteSpace(screen.ScreenName))
            throw new ArgumentException("Screen name is required.");

        var existingScreen = await _repository.GetByIdAsync(id);

        if (existingScreen == null)
            return false;

        existingScreen.TheatreId = screen.TheatreId;
        existingScreen.ScreenName = screen.ScreenName.Trim();

        await _repository.UpdateAsync(existingScreen);

        return true;
    }

    public async Task<bool> DeleteScreenAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid screen ID.");

        var screen = await _repository.GetByIdAsync(id);

        if (screen == null)
            return false;

        await _repository.DeleteAsync(screen);

        return true;
    }
}