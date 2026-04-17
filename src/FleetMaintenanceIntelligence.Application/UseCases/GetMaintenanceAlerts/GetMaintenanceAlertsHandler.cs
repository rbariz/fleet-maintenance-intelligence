using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Contracts.MaintenanceAlerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetMaintenanceIntelligence.Application.UseCases.GetMaintenanceAlerts
{
    
    public sealed class GetMaintenanceAlertsHandler
    {
        private readonly IMaintenanceAlertRepository _alertRepository;

        public GetMaintenanceAlertsHandler(IMaintenanceAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task<IReadOnlyList<MaintenanceAlertResponse>> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await _alertRepository.GetAllAsync(cancellationToken);

            return items
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(x => new MaintenanceAlertResponse(
                    x.Id,
                    x.VehicleId,
                    x.Title,
                    x.Description,
                    x.Severity.ToString().ToLowerInvariant(),
                    x.Status.ToString().ToLowerInvariant(),
                    x.CreatedAtUtc,
                    x.ResolvedAtUtc))
                .ToList();
        }
    }
}
