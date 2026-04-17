using FleetMaintenanceIntelligence.Contracts.MaintenanceAlerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetMaintenanceIntelligence.Contracts.Dashboard
{
    public sealed record FleetDashboardResponse(
    Guid DemoVehicleId,
    int TotalAlerts,
    int OpenAlerts,
    IReadOnlyList<MaintenanceAlertResponse> LatestAlerts
);
}
