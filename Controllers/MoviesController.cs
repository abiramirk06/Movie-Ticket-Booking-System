
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Services.Interfaces;

namespace MovieTicketBookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET: api/movies
    // Public access
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await _movieService.GetAllMoviesAsync();

        return Ok(movies);
    }

    // GET: api/movies/1
    // Public access
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);

        if (movie == null)
            return NotFound(new { message = "Movie not found." });

        return Ok(movie);
    }

    // POST: api/movies
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateMovie(Movie movie)
    {
        var createdMovie = await _movieService.CreateMovieAsync(movie);

        return CreatedAtAction(
            nameof(GetMovieById),
            new { id = createdMovie.MovieId },
            createdMovie);
    }

    // PUT: api/movies/1
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, Movie movie)
    {
        await _movieService.UpdateMovieAsync(id, movie);

        return NoContent();
    }

    // DELETE: api/movies/1
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        await _movieService.DeleteMovieAsync(id);

        return NoContent();
    }
}
