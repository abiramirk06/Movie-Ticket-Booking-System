using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieTicketBookingSystem.API.Models;

public class BookingSeat
{
    public int BookingSeatId { get; set; }

    [Required]
    public int BookingId { get; set; }

    [Required]
    public int SeatId { get; set; }

    public decimal Price { get; set; }

    [JsonIgnore]
    public Booking? Booking { get; set; }

    [JsonIgnore]
    public Seat? Seat { get; set; }
}