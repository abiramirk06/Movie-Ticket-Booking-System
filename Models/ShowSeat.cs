using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieTicketBookingSystem.API.Models;

public class ShowSeat
{
    public int ShowSeatId { get; set; }

    [Required]
    public int ShowId { get; set; }

    [Required]
    public int SeatId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Available";

    [JsonIgnore]
    public Show? Show { get; set; }

    [JsonIgnore]
    public Seat? Seat { get; set; }
}