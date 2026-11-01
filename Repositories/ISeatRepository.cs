using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Repositories;

public interface ISeatRepository
{
    Task<List<Seat>> GetAllAsync();
    Task<Seat?> GetByIdAsync(int id);
    Task AddAsync(Seat seat);
    Task UpdateAsync(Seat seat);
    Task DeleteAsync(Seat seat);
}