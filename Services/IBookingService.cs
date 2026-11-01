using MovieTicketBookingSystem.API.DTOs;

namespace MovieTicketBookingSystem.API.Services;

public interface IBookingService
{
    Task<BookingResponse?> GetBookingByIdAsync(int id);

    Task<BookingResponse?> CreateBookingAsync(
        CreateBookingRequest request);

    Task<bool> CancelBookingAsync(int id);
}