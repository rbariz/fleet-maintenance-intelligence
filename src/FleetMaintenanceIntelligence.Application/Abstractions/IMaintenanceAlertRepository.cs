using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Application.Abstractions
{
    public interface IMaintenanceAlertRepository
    {
        Task AddAsync(MaintenanceAlert alert, CancellationToken cancellationToken = default);
    }
}
