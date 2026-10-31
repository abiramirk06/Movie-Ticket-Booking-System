using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Services.Interfaces;

namespace MovieTicketBookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TheatresController : ControllerBase
{
    private readonly ITheatreService _theatreService;

    public TheatresController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTheatres()
    {
        var theatres = await _theatreService.GetAllTheatresAsync();
        return Ok(theatres);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTheatreById(int id)
    {
        var theatre = await _theatreService.GetTheatreByIdAsync(id);

        if (theatre == null)
            return NotFound(new { message = "Theatre not found." });

        return Ok(theatre);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTheatre(Theatre theatre)
    {
        var createdTheatre = await _theatreService.CreateTheatreAsync(theatre);

        return CreatedAtAction(
            nameof(GetTheatreById),
            new { id = createdTheatre.TheatreId },
            createdTheatre);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTheatre(int id, Theatre theatre)
    {
        await _theatreService.UpdateTheatreAsync(id, theatre);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTheatre(int id)
    {
        await _theatreService.DeleteTheatreAsync(id);
        return NoContent();
    }
}