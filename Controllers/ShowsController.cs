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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllShowsAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var show = await _service.GetShowByIdAsync(id);

        if (show == null)
            return NotFound();

        return Ok(show);
    }

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

    [HttpPost]
    public async Task<IActionResult> Create(Show show)
    {
        var createdShow = await _service.CreateShowAsync(show);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdShow.ShowId },
            createdShow);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Show show)
    {
        var updated = await _service.UpdateShowAsync(id, show);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteShowAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}