using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly ITripsService _tripsService;

    public ClientsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientDTO dto)
    {
        if (IsClientDtoInvalid(dto))
        {
            return BadRequest("All fields are required.");
        }

        try
        {
            var createdClientId = await _tripsService.CreateClientAsync(dto);
            return CreatedAtAction(nameof(CreateClient), new { id = createdClientId }, new { Id = createdClientId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the client.");
        }
    }

    private static bool IsClientDtoInvalid(CreateClientDTO dto)
    {
        return string.IsNullOrWhiteSpace(dto.FirstName) ||
               string.IsNullOrWhiteSpace(dto.LastName) ||
               string.IsNullOrWhiteSpace(dto.Email) ||
               string.IsNullOrWhiteSpace(dto.Telephone) ||
               string.IsNullOrWhiteSpace(dto.Pesel);
    }
}