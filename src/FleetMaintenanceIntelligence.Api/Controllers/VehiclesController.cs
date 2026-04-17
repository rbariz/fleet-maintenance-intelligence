using FleetMaintenanceIntelligence.Application.UseCases.GetVehicleTimeline;
using FleetMaintenanceIntelligence.Contracts.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace FleetMaintenanceIntelligence.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class VehiclesController : ControllerBase
    {
        private readonly GetVehicleTimelineHandler _getVehicleTimelineHandler;

        public VehiclesController(GetVehicleTimelineHandler getVehicleTimelineHandler)
        {
            _getVehicleTimelineHandler = getVehicleTimelineHandler;
        }

        [HttpGet("{vehicleId:guid}/timeline")]
        [ProducesResponseType(typeof(IReadOnlyList<VehicleTimelineItemResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTimeline(Guid vehicleId, CancellationToken cancellationToken)
        {
            var result = await _getVehicleTimelineHandler.HandleAsync(vehicleId, cancellationToken);
            return Ok(result);
        }
    }
}
