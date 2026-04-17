using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Application.Abstractions
{
    public interface IFleetVehicleRepository
    {
        Task<FleetVehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
