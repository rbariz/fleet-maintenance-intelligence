using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Application.Abstractions
{
    public interface IVehicleTelemetrySnapshotRepository
    {
        Task<VehicleTelemetrySnapshot?> GetLatestByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
    }
}
