using FleetMaintenanceIntelligence.Application.UseCases.AcknowledgeMaintenanceAlert;
using FleetMaintenanceIntelligence.Application.UseCases.EvaluateMaintenanceAlerts;
using FleetMaintenanceIntelligence.Application.UseCases.GetMaintenanceAlerts;
using FleetMaintenanceIntelligence.Application.UseCases.ResolveMaintenanceAlert;
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

        private readonly AcknowledgeMaintenanceAlertHandler _acknowledgeHandler;
        private readonly ResolveMaintenanceAlertHandler _resolveHandler;

        public MaintenanceAlertsController(
            EvaluateMaintenanceAlertsHandler evaluateHandler,
            GetMaintenanceAlertsHandler getHandler,
            AcknowledgeMaintenanceAlertHandler acknowledgeHandler,
            ResolveMaintenanceAlertHandler resolveHandler)
        {
            _evaluateHandler = evaluateHandler;
            _getHandler = getHandler;
            _acknowledgeHandler = acknowledgeHandler;
            _resolveHandler = resolveHandler;
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

        [HttpPost("{alertId:guid}/acknowledge")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Acknowledge(Guid alertId, CancellationToken cancellationToken)
        {
            await _acknowledgeHandler.HandleAsync(alertId, cancellationToken);
            return NoContent();
        }

        [HttpPost("{alertId:guid}/resolve")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Resolve(Guid alertId, CancellationToken cancellationToken)
        {
            await _resolveHandler.HandleAsync(alertId, cancellationToken);
            return NoContent();
        }
    }
}
