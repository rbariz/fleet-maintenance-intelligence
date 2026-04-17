using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Application.Abstractions
{
    public interface IMaintenanceAlertRepository
    {
        Task AddAsync(MaintenanceAlert alert, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<MaintenanceAlert>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsOpenAlertAsync(
    Guid vehicleId,
    string title,
    CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MaintenanceAlert>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
    }
}
