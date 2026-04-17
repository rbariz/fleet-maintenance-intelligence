using FleetMaintenanceIntelligence.Application.UseCases.GetFleetDashboard;
using FleetMaintenanceIntelligence.Contracts.Dashboard;
using FleetMaintenanceIntelligence.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Mvc;


namespace FleetMaintenanceIntelligence.Api.Controllers
{
    [ApiController]
    [Route("api/fleet-dashboard")]
    public sealed class FleetDashboardController : ControllerBase
    {
        private readonly GetFleetDashboardHandler _handler;

        public FleetDashboardController(GetFleetDashboardHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        [ProducesResponseType(typeof(FleetDashboardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _handler.HandleAsync(InMemorySeed.DemoVehicleId, cancellationToken);
            return Ok(result);
        }
    }
}
