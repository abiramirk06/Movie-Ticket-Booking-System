
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Services;

namespace MovieTicketBookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShowsController : ControllerBase
{
    private readonly IShowService _service;

    public ShowsController(IShowService service)
    {
        _service = service;
    }

    // GET: api/shows
    // Public access
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllShowsAsync());
    }

    // GET: api/shows/1
    // Public access
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var show = await _service.GetShowByIdAsync(id);

        if (show == null)
            return NotFound();

        return Ok(show);
    }

    // GET: api/shows/1/seats
    // Public access - view available and booked seats
    [AllowAnonymous]
    [HttpGet("{id}/seats")]
    public async Task<IActionResult> GetShowSeats(int id)
    {
        var seats = await _service.GetShowSeatsAsync(id);

        if (seats == null)
            return NotFound(new { message = "Show not found." });

        var result = seats.Select(ss => new
        {
            seatId = ss.SeatId,
            rowName = ss.Seat!.RowName,
            seatNumber = ss.Seat.SeatNumber,
            seatType = ss.Seat.SeatType,
            status = ss.Status
        });

        return Ok(result);
    }

    // POST: api/shows
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Show show)
    {
        var createdShow = await _service.CreateShowAsync(show);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdShow.ShowId },
            createdShow);
    }

    // PUT: api/shows/1
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Show show)
    {
        var updated = await _service.UpdateShowAsync(id, show);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/shows/1
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteShowAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
