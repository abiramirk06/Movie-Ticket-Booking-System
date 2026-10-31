using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Services.Interfaces;

public interface ITheatreService
{
    Task<IEnumerable<Theatre>> GetAllTheatresAsync();

    Task<Theatre?> GetTheatreByIdAsync(int id);

    Task<Theatre> CreateTheatreAsync(Theatre theatre);

    Task UpdateTheatreAsync(int id, Theatre theatre);

    Task DeleteTheatreAsync(int id);
}