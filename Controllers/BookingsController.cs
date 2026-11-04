using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    // Customers can view their own bookings.
    // Admins can view any booking.
    [Authorize(Roles = "Admin,Customer")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var booking = await _service.GetBookingByIdAsync(
            id,
            userId,
            isAdmin);

        if (booking == null)
            return NotFound(new { message = "Booking not found." });

        return Ok(booking);
    }

    // POST: api/bookings
    // Only authenticated customers can create bookings.
    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookingRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            var booking = await _service.CreateBookingAsync(
                request,
                userId);

            if (booking == null)
            {
                return Conflict(new
                {
                    message = "Show not found or one or more seats are unavailable."
                });
            }

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
    // Customers can cancel their own bookings.
    // Admins can cancel any booking.
    [Authorize(Roles = "Admin,Customer")]
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var cancelled = await _service.CancelBookingAsync(
            id,
            userId,
            isAdmin);

        if (!cancelled)
        {
            return NotFound(new
            {
                message = "Booking not found, not accessible, or already cancelled."
            });
        }

        return Ok(new
        {
            message = "Booking cancelled successfully."
        });
    }

    // GET: api/bookings/my-bookings
    // Customer can view only their own bookings.
    [Authorize(Roles = "Customer")]
    [HttpGet("my-bookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var bookings = await _service.GetMyBookingsAsync(userId);

        return Ok(bookings);
    }
}