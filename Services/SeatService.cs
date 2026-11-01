using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Repositories;

namespace MovieTicketBookingSystem.API.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _repository;

    public SeatService(ISeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Seat>> GetAllSeatsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Seat?> GetSeatByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid seat ID.");

        return await _repository.GetByIdAsync(id);
    }

    public async Task<Seat> CreateSeatAsync(Seat seat)
    {
        if (seat.ScreenId <= 0)
            throw new ArgumentException("Valid ScreenId is required.");

        if (string.IsNullOrWhiteSpace(seat.RowName))
            throw new ArgumentException("Row name is required.");

        if (seat.SeatNumber <= 0)
            throw new ArgumentException("Seat number must be greater than zero.");

        if (string.IsNullOrWhiteSpace(seat.SeatType))
            throw new ArgumentException("Seat type is required.");

        seat.RowName = seat.RowName.Trim().ToUpper();
        seat.SeatType = seat.SeatType.Trim();

        await _repository.AddAsync(seat);

        return seat;
    }

    public async Task<bool> UpdateSeatAsync(int id, Seat seat)
    {
        if (id <= 0 || seat.ScreenId <= 0)
            throw new ArgumentException("Invalid seat or screen ID.");

        if (string.IsNullOrWhiteSpace(seat.RowName) ||
            string.IsNullOrWhiteSpace(seat.SeatType) ||
            seat.SeatNumber <= 0)
        {
            throw new ArgumentException("Invalid seat details.");
        }

        var existingSeat = await _repository.GetByIdAsync(id);

        if (existingSeat == null)
            return false;

        existingSeat.ScreenId = seat.ScreenId;
        existingSeat.RowName = seat.RowName.Trim().ToUpper();
        existingSeat.SeatNumber = seat.SeatNumber;
        existingSeat.SeatType = seat.SeatType.Trim();

        await _repository.UpdateAsync(existingSeat);

        return true;
    }

    public async Task<bool> DeleteSeatAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid seat ID.");

        var seat = await _repository.GetByIdAsync(id);

        if (seat == null)
            return false;

        await _repository.DeleteAsync(seat);

        return true;
    }
}