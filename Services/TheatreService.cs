using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Repositories.Interfaces;
using MovieTicketBookingSystem.API.Services.Interfaces;

namespace MovieTicketBookingSystem.API.Services;

public class TheatreService : ITheatreService
{
    private readonly ITheatreRepository _theatreRepository;

    public TheatreService(ITheatreRepository theatreRepository)
    {
        _theatreRepository = theatreRepository;
    }

    public async Task<IEnumerable<Theatre>> GetAllTheatresAsync()
    {
        return await _theatreRepository.GetAllAsync();
    }

    public async Task<Theatre?> GetTheatreByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid theatre ID.");

        return await _theatreRepository.GetByIdAsync(id);
    }

    public async Task<Theatre> CreateTheatreAsync(Theatre theatre)
    {
        if (string.IsNullOrWhiteSpace(theatre.Name))
            throw new ArgumentException("Theatre name is required.");

        if (string.IsNullOrWhiteSpace(theatre.Location))
            throw new ArgumentException("Theatre location is required.");

        return await _theatreRepository.AddAsync(theatre);
    }

    public async Task UpdateTheatreAsync(int id, Theatre theatre)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid theatre ID.");

        var existingTheatre = await _theatreRepository.GetByIdAsync(id);

        if (existingTheatre == null)
            throw new KeyNotFoundException("Theatre not found.");

        existingTheatre.Name = theatre.Name;
        existingTheatre.Location = theatre.Location;

        await _theatreRepository.UpdateAsync(existingTheatre);
    }

    public async Task DeleteTheatreAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid theatre ID.");

        var theatre = await _theatreRepository.GetByIdAsync(id);

        if (theatre == null)
            throw new KeyNotFoundException("Theatre not found.");

        await _theatreRepository.DeleteAsync(theatre);
    }
}