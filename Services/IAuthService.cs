using MovieTicketBookingSystem.API.DTOs;

namespace MovieTicketBookingSystem.API.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);
}