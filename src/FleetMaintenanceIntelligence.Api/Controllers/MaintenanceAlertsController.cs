using FleetMaintenanceIntelligence.Application.UseCases.EvaluateMaintenanceAlerts;
using FleetMaintenanceIntelligence.Application.UseCases.GetMaintenanceAlerts;
using FleetMaintenanceIntelligence.Contracts.MaintenanceAlerts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FleetMaintenanceIntelligence.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class MaintenanceAlertsController : ControllerBase
    {
        private readonly EvaluateMaintenanceAlertsHandler _evaluateHandler;
        private readonly GetMaintenanceAlertsHandler _getHandler;

        public MaintenanceAlertsController(
            EvaluateMaintenanceAlertsHandler evaluateHandler,
            GetMaintenanceAlertsHandler getHandler)
        {
            _evaluateHandler = evaluateHandler;
            _getHandler = getHandler;
        }

        [HttpPost("evaluate/{vehicleId:guid}")]
        [ProducesResponseType(typeof(EvaluateMaintenanceAlertsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Evaluate(Guid vehicleId, CancellationToken cancellationToken)
        {
            var result = await _evaluateHandler.HandleAsync(
                new EvaluateMaintenanceAlertsRequest(vehicleId),
                cancellationToken);

            var response = new EvaluateMaintenanceAlertsResponse(
                result.VehicleId,
                result.CreatedAlertsCount,
                result.CreatedAlertTitles);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<MaintenanceAlertResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _getHandler.HandleAsync(cancellationToken);
            return Ok(result);
        }
    }
}
