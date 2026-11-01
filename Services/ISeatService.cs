using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Services;

    public interface ISeatService
    {
        Task<List<Seat>> GetAllSeatsAsync();
        Task<Seat?> GetSeatByIdAsync(int id);
        Task<Seat> CreateSeatAsync(Seat seat);
        Task<bool> UpdateSeatAsync(int id, Seat seat);
        Task<bool> DeleteSeatAsync(int id);
    }