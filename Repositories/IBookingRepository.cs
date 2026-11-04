using MovieTicketBookingSystem.API.DTOs;
using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(
        int id,
        string userId,
        bool isAdmin);

    Task<List<Booking>> GetMyBookingsAsync(string userId);

    Task<Booking?> CreateBookingAsync(
        Booking booking,
        List<int> seatIds);

    Task<bool> CancelBookingAsync(
        int bookingId,
        string userId,
        bool isAdmin);
}