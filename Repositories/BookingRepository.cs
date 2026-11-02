using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.API.Data;
using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(
        int id,
        string userId,
        bool isAdmin)
    {
        var query = _context.Bookings
            .Include(b => b.BookingSeats)
            .Where(b => b.BookingId == id);

        if (!isAdmin)
        {
            query = query.Where(b => b.UserId == userId);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Booking?> CreateBookingAsync(
        Booking booking,
        List<int> seatIds)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var show = await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == booking.ShowId);

            if (show == null)
                return null;

            var showSeats = await _context.ShowSeats
                .Where(ss =>
                    ss.ShowId == booking.ShowId &&
                    seatIds.Contains(ss.SeatId))
                .ToListAsync();

            if (showSeats.Count != seatIds.Count ||
                showSeats.Any(ss => ss.Status != "Available"))
            {
                return null;
            }

            booking.TotalAmount = show.TicketPrice * seatIds.Count;
            booking.BookingDate = DateTime.Now;
            booking.Status = "Confirmed";

            foreach (var seatId in seatIds)
            {
                booking.BookingSeats.Add(new BookingSeat
                {
                    SeatId = seatId,
                    Price = show.TicketPrice
                });
            }

            await _context.Bookings.AddAsync(booking);

            foreach (var showSeat in showSeats)
            {
                showSeat.Status = "Booked";
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return booking;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> CancelBookingAsync(
        int bookingId,
        string userId,
        bool isAdmin)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var query = _context.Bookings
                .Include(b => b.BookingSeats)
                .Where(b => b.BookingId == bookingId);

            if (!isAdmin)
            {
                query = query.Where(b => b.UserId == userId);
            }

            var booking = await query.FirstOrDefaultAsync();

            if (booking == null || booking.Status != "Confirmed")
            {
                return false;
            }

            var seatIds = booking.BookingSeats
                .Select(bs => bs.SeatId)
                .ToList();

            var showSeats = await _context.ShowSeats
                .Where(ss =>
                    ss.ShowId == booking.ShowId &&
                    seatIds.Contains(ss.SeatId))
                .ToListAsync();

            booking.Status = "Cancelled";

            foreach (var showSeat in showSeats)
            {
                showSeat.Status = "Available";
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}