using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Contracts.Dashboard;
using FleetMaintenanceIntelligence.Contracts.MaintenanceAlerts;
using FleetMaintenanceIntelligence.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetMaintenanceIntelligence.Application.UseCases.GetFleetDashboard
{
    public sealed class GetFleetDashboardHandler
    {
        private readonly IMaintenanceAlertRepository _alertRepository;

        public GetFleetDashboardHandler(IMaintenanceAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task<FleetDashboardResponse> HandleAsync(
            Guid demoVehicleId,
            CancellationToken cancellationToken = default)
        {
            var items = await _alertRepository.GetAllAsync(cancellationToken);

            var latest = items
                .OrderByDescending(x => x.CreatedAtUtc)
                .Take(20)
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

            return new FleetDashboardResponse(
                demoVehicleId,
                items.Count,
                items.Count(x => x.Status == AlertStatus.Open),
                latest);
        }
    }
}
