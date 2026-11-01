using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(int id);

    Task<Booking?> CreateBookingAsync(
        Booking booking,
        List<int> seatIds);

    Task<bool> CancelBookingAsync(int bookingId);
}