using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Repositories;

namespace MovieTicketBookingSystem.API.Services;

public class ShowService : IShowService
{
    private readonly IShowRepository _repository;

    public ShowService(IShowRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Show>> GetAllShowsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Show?> GetShowByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid Show ID.");

        return await _repository.GetByIdAsync(id);
    }

    public async Task<Show> CreateShowAsync(Show show)
    {
        if (show.MovieId <= 0)
            throw new ArgumentException("Valid MovieId is required.");

        if (show.ScreenId <= 0)
            throw new ArgumentException("Valid ScreenId is required.");

        if (show.StartTime <= DateTime.Now)
            throw new ArgumentException("Show start time must be in the future.");

        if (show.EndTime <= show.StartTime)
            throw new ArgumentException("End time must be after start time.");

        if (show.TicketPrice < 0)
            throw new ArgumentException("Ticket price cannot be negative.");

        if (!await _repository.MovieExistsAsync(show.MovieId))
            throw new ArgumentException("Movie does not exist.");

        if (!await _repository.ScreenExistsAsync(show.ScreenId))
            throw new ArgumentException("Screen does not exist.");

        return await _repository.AddAsync(show);
    }

    public async Task<bool> UpdateShowAsync(int id, Show show)
    {
        if (id <= 0 || show.MovieId <= 0 || show.ScreenId <= 0)
            throw new ArgumentException("Invalid show, movie or screen ID.");

        if (show.EndTime <= show.StartTime)
            throw new ArgumentException("End time must be after start time.");

        if (show.TicketPrice < 0)
            throw new ArgumentException("Ticket price cannot be negative.");

        if (!await _repository.MovieExistsAsync(show.MovieId))
            throw new ArgumentException("Movie does not exist.");

        if (!await _repository.ScreenExistsAsync(show.ScreenId))
            throw new ArgumentException("Screen does not exist.");

        var existingShow = await _repository.GetByIdAsync(id);

        if (existingShow == null)
            return false;

        existingShow.MovieId = show.MovieId;
        existingShow.ScreenId = show.ScreenId;
        existingShow.StartTime = show.StartTime;
        existingShow.EndTime = show.EndTime;
        existingShow.TicketPrice = show.TicketPrice;

        await _repository.UpdateAsync(existingShow);

        return true;
    }

    public async Task<bool> DeleteShowAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid Show ID.");

        var show = await _repository.GetByIdAsync(id);

        if (show == null)
            return false;

        await _repository.DeleteAsync(show);

        return true;
    }
}