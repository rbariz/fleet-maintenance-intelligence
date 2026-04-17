using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Application.Abstractions
{
    public interface IMaintenancePlanRepository
    {
        Task<IReadOnlyList<MaintenancePlan>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
    }
}
