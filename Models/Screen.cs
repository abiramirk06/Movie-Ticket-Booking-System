using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieTicketBookingSystem.API.Models;

public class Screen
{
    public int ScreenId { get; set; }

    [Required]
    public int TheatreId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ScreenName { get; set; } = string.Empty;

    [JsonIgnore]
    public Theatre? Theatre { get; set; }
}