using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieTicketBookingSystem.API.Models;

public class Booking
{
    public int BookingId { get; set; }

    [Required]
    [MaxLength(30)]
    public string BookingReference { get; set; } = string.Empty;

    [Required]
    public int ShowId { get; set; }

    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(15)]
    public string CustomerPhone { get; set; } = string.Empty;

    public DateTime BookingDate { get; set; }

    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Confirmed";

    [JsonIgnore]
    public Show? Show { get; set; }

    public ICollection<BookingSeat> BookingSeats { get; set; }
        = new List<BookingSeat>();

    // Authenticated user who created this booking
    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }
}