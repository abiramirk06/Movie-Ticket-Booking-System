
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Services;

namespace MovieTicketBookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _service;

    public SeatsController(ISeatService service)
    {
        _service = service;
    }

    // GET: api/seats
    // Public access
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllSeatsAsync());
    }

    // GET: api/seats/1
    // Public access
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var seat = await _service.GetSeatByIdAsync(id);

        if (seat == null)
            return NotFound();

        return Ok(seat);
    }

    // POST: api/seats
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Seat seat)
    {
        var createdSeat = await _service.CreateSeatAsync(seat);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdSeat.SeatId },
            createdSeat);
    }

    // PUT: api/seats/1
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Seat seat)
    {
        var updated = await _service.UpdateSeatAsync(id, seat);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/seats/1
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteSeatAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
