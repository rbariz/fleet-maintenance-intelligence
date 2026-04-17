using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Infrastructure.Persistence;

public sealed class InMemoryFleetVehicleRepository : IFleetVehicleRepository
{
    private readonly InMemoryStore _store;

    public InMemoryFleetVehicleRepository(InMemoryStore store)
    {
        _store = store;
    }

    public Task<FleetVehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_store.Vehicles.FirstOrDefault(x => x.Id == id));
    }
}
