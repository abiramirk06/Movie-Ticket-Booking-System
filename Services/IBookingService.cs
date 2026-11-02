using MovieTicketBookingSystem.API.DTOs;

namespace MovieTicketBookingSystem.API.Services;

public interface IBookingService
{
    Task<BookingResponse?> GetBookingByIdAsync(
        int id,
        string userId,
        bool isAdmin);

    Task<BookingResponse?> CreateBookingAsync(
        CreateBookingRequest request,
        string userId);

    Task<bool> CancelBookingAsync(
        int id,
        string userId,
        bool isAdmin);
}