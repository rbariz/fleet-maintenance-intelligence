using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Application.Abstractions
{
    public interface IVehicleTelemetrySnapshotRepository
    {
        Task<VehicleTelemetrySnapshot?> GetLatestByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<VehicleTelemetrySnapshot>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
    }
}
