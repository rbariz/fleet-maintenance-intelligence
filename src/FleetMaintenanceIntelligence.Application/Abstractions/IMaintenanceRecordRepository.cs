using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Application.Abstractions
{
    public interface IMaintenanceRecordRepository
    {
        Task<IReadOnlyList<MaintenanceRecord>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
    }
}
