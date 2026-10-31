using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Repositories.Interfaces;
using MovieTicketBookingSystem.API.Services.Interfaces;

namespace MovieTicketBookingSystem.API.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        return await _movieRepository.GetAllAsync();
    }

    public async Task<Movie?> GetMovieByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        return await _movieRepository.GetByIdAsync(id);
    }

    public async Task<Movie> CreateMovieAsync(Movie movie)
    {
        if (string.IsNullOrWhiteSpace(movie.Title))
            throw new ArgumentException("Movie title is required.");

        if (movie.DurationMinutes <= 0)
            throw new ArgumentException("Movie duration must be greater than zero.");

        return await _movieRepository.AddAsync(movie);
    }

    public async Task UpdateMovieAsync(int id, Movie movie)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        var existingMovie = await _movieRepository.GetByIdAsync(id);

        if (existingMovie == null)
            throw new KeyNotFoundException("Movie not found.");

        existingMovie.Title = movie.Title;
        existingMovie.Description = movie.Description;
        existingMovie.DurationMinutes = movie.DurationMinutes;
        existingMovie.ReleaseDate = movie.ReleaseDate;
        existingMovie.Language = movie.Language;
        existingMovie.Genre = movie.Genre;

        await _movieRepository.UpdateAsync(existingMovie);
    }

    public async Task DeleteMovieAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie == null)
            throw new KeyNotFoundException("Movie not found.");

        await _movieRepository.DeleteAsync(movie);
    }
}