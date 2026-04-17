using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetMaintenanceIntelligence.Contracts.MaintenanceAlerts
{
    public sealed record MaintenanceAlertResponse(
     Guid Id,
     Guid VehicleId,
     string Title,
     string Description,
     string Severity,
     string Status,
     DateTime CreatedAtUtc,
     DateTime? ResolvedAtUtc
 );
}
