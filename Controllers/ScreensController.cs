using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Services;

namespace MovieTicketBookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScreensController : ControllerBase
{
    private readonly IScreenService _service;

    public ScreensController(IScreenService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllScreensAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var screen = await _service.GetScreenByIdAsync(id);

        if (screen == null)
            return NotFound();

        return Ok(screen);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Screen screen)
    {
        var createdScreen = await _service.CreateScreenAsync(screen);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdScreen.ScreenId },
            createdScreen
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Screen screen)
    {
        var updated = await _service.UpdateScreenAsync(id, screen);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteScreenAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}