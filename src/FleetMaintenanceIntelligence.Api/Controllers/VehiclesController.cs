using FleetMaintenanceIntelligence.Application.UseCases.GetVehicleSummary;
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
        private readonly GetVehicleSummaryHandler _getVehicleSummaryHandler;

        public VehiclesController(GetVehicleTimelineHandler getVehicleTimelineHandler, GetVehicleSummaryHandler getVehicleSummaryHandler)
        {
            _getVehicleTimelineHandler = getVehicleTimelineHandler;
            _getVehicleSummaryHandler = getVehicleSummaryHandler;
        }

        [HttpGet("{vehicleId:guid}/timeline")]
        [ProducesResponseType(typeof(IReadOnlyList<VehicleTimelineItemResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTimeline(Guid vehicleId, CancellationToken cancellationToken)
        {
            var result = await _getVehicleTimelineHandler.HandleAsync(vehicleId, cancellationToken);
            return Ok(result);
        }


        [HttpGet("{vehicleId:guid}/summary")]
        [ProducesResponseType(typeof(VehicleSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSummary(Guid vehicleId, CancellationToken cancellationToken)
        {
            var result = await _getVehicleSummaryHandler.HandleAsync(vehicleId, cancellationToken);
            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
