using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieTicketBookingSystem.API.Models;

public class Seat
{
    public int SeatId { get; set; }

    [Required]
    public int ScreenId { get; set; }

    [Required]
    [MaxLength(5)]
    public string RowName { get; set; } = string.Empty;

    [Required]
    public int SeatNumber { get; set; }

    [Required]
    [MaxLength(30)]
    public string SeatType { get; set; } = string.Empty;

    [JsonIgnore]
    public Screen? Screen { get; set; }
}