using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.API.Models;
using MovieTicketBookingSystem.API.Services;

namespace MovieTicketBookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShowsController : ControllerBase
{
    private readonly IShowService _service;

    public ShowsController(IShowService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllShowsAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var show = await _service.GetShowByIdAsync(id);

        if (show == null)
            return NotFound();

        return Ok(show);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Show show)
    {
        var createdShow = await _service.CreateShowAsync(show);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdShow.ShowId },
            createdShow);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Show show)
    {
        var updated = await _service.UpdateShowAsync(id, show);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteShowAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}