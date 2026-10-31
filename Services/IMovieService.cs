using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Services.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetAllMoviesAsync();
    Task<Movie?> GetMovieByIdAsync(int id);
    Task<Movie> CreateMovieAsync(Movie movie);
    Task UpdateMovieAsync(int id, Movie movie);
    Task DeleteMovieAsync(int id);
}