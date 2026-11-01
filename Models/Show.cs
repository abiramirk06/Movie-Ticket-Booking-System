using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieTicketBookingSystem.API.Models;

public class Show
{
    public int ShowId { get; set; }

    [Required]
    public int MovieId { get; set; }

    [Required]
    public int ScreenId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public decimal TicketPrice { get; set; }

    [JsonIgnore]
    public Movie? Movie { get; set; }

    [JsonIgnore]
    public Screen? Screen { get; set; }

    [JsonIgnore]
    public ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();
}