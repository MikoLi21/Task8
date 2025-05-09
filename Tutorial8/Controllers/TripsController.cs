using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Services;
using System.Threading.Tasks;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;

        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var result = await _tripsService.GetTrips();
            return result is not null ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            // var exists = await DoesTripExist(id);
            // if (!exists) return NotFound();
            // var trip = await GetTrip(id);
            // return Ok(trip);

            var allTrips = await _tripsService.GetTrips(); // Заглушка
            return Ok(allTrips);
        }

        [HttpGet("/api/clients/{id}/trips")]
        public async Task<IActionResult> GetTripsForClient(int id)
        {
            var clientTrips = await _tripsService.GetTripsByClientId(id);

            return clientTrips is { Count: > 0 }
                ? Ok(clientTrips)
                : NotFound($"No trips found for client with ID {id}.");
        }

        [HttpPut("/api/clients/{id}/trips/{tripId}")]
        public async Task<IActionResult> RegisterClientForTrip(int id, int tripId)
        {
            var registrationResult = await _tripsService.RegisterClientForTripAsync(id, tripId);

            return registrationResult switch
            {
                null => Ok("Client successfully registered for the trip."),
                "Client not found" => NotFound("Client not found."),
                "Trip not found" => NotFound("Trip not found."),
                "Client already registered" => Conflict("Client already registered for this trip."),
                "Max participants reached" => Conflict("Maximum number of participants reached."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error.")
            };
        }

        [HttpDelete("/api/clients/{id}/trips/{tripId}")]
        public async Task<IActionResult> UnregisterClientFromTrip(int id, int tripId)
        {
            var unregistrationResult = await _tripsService.UnregisterClientFromTripAsync(id, tripId);

            return unregistrationResult switch
            {
                null => Ok("Client successfully unregistered from the trip."),
                "Registration not found" => NotFound("Client is not registered for this trip."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred.")
            };
        }
    }
}