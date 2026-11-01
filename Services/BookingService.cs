using MovieTicketBookingSystem.API.DTOs;
using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Repositories;

namespace MovieTicketBookingSystem.API.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _repository;

    public BookingService(IBookingRepository repository)
    {
        _repository = repository;
    }

    public async Task<BookingResponse?> GetBookingByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        var booking = await _repository.GetByIdAsync(id);

        return booking == null ? null : MapToResponse(booking);
    }

    public async Task<BookingResponse?> CreateBookingAsync(
        CreateBookingRequest request)
    {
        if (request.ShowId <= 0 ||
            string.IsNullOrWhiteSpace(request.CustomerName) ||
            string.IsNullOrWhiteSpace(request.CustomerEmail) ||
            string.IsNullOrWhiteSpace(request.CustomerPhone) ||
            request.SeatIds == null ||
            request.SeatIds.Count == 0 ||
            request.SeatIds.Any(id => id <= 0) ||
            request.SeatIds.Distinct().Count() != request.SeatIds.Count)
        {
            throw new ArgumentException("Invalid booking details.");
        }

        var booking = new Booking
        {
            BookingReference = Guid.NewGuid()
                .ToString("N")[..20].ToUpperInvariant(),

            ShowId = request.ShowId,
            CustomerName = request.CustomerName.Trim(),
            CustomerEmail = request.CustomerEmail.Trim(),
            CustomerPhone = request.CustomerPhone.Trim()
        };

        var createdBooking = await _repository.CreateBookingAsync(
            booking,
            request.SeatIds);

        return createdBooking == null
            ? null
            : MapToResponse(createdBooking);
    }

    public async Task<bool> CancelBookingAsync(int id)
    {
        if (id <= 0)
            return false;

        return await _repository.CancelBookingAsync(id);
    }

    private static BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            ShowId = booking.ShowId,
            CustomerName = booking.CustomerName,
            CustomerEmail = booking.CustomerEmail,
            CustomerPhone = booking.CustomerPhone,
            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            SeatIds = booking.BookingSeats
                .Select(bs => bs.SeatId)
                .ToList()
        };
    }
}