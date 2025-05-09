using Microsoft.AspNetCore.Mvc;
using Tutorial8.Services;

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
            var trips = await _tripsService.GetTrips();
            return Ok(trips);
        }

        [HttpGet("/api/clients/{id}/trips")]
        public async Task<IActionResult> GetTripsForClient(int id)
        {
            var trips = await _tripsService.GetTripsByClientId(id);

            if (trips.Count == 0)
            {
                return NotFound($"No trips found for client with ID {id}.");
            }

            return Ok(trips);
        }
    }
}