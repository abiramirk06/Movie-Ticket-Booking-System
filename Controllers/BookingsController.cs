using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.API.DTOs;
using MovieTicketBookingSystem.API.Services;

namespace MovieTicketBookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _service;

    public BookingsController(IBookingService service)
    {
        _service = service;
    }

    // GET: api/bookings/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _service.GetBookingByIdAsync(id);

        if (booking == null)
            return NotFound(new { message = "Booking not found." });

        return Ok(booking);
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookingRequest request)
    {
        try
        {
            var booking = await _service.CreateBookingAsync(request);

            if (booking == null)
                return Conflict(new
                {
                    message = "Show not found or one or more seats are unavailable."
                });

            return CreatedAtAction(
                nameof(GetById),
                new { id = booking.BookingId },
                booking);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/bookings/1/cancel
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var cancelled = await _service.CancelBookingAsync(id);

        if (!cancelled)
            return NotFound(new
            {
                message = "Booking not found or already cancelled."
            });

        return Ok(new
        {
            message = "Booking cancelled successfully."
        });
    }
}