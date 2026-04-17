using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Infrastructure.Persistence;

public sealed class InMemoryMaintenancePlanRepository : IMaintenancePlanRepository
{
    private readonly InMemoryStore _store;

    public InMemoryMaintenancePlanRepository(InMemoryStore store)
    {
        _store = store;
    }

    public Task<IReadOnlyList<MaintenancePlan>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var items = _store.MaintenancePlans
            .Where(x => x.VehicleId == vehicleId)
            .ToList();

        return Task.FromResult<IReadOnlyList<MaintenancePlan>>(items);
    }
}
