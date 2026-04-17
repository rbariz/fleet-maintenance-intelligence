using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Infrastructure.Persistence;

public sealed class InMemoryMaintenanceRecordRepository : IMaintenanceRecordRepository
{
    private readonly InMemoryStore _store;

    public InMemoryMaintenanceRecordRepository(InMemoryStore store)
    {
        _store = store;
    }

    public Task<IReadOnlyList<MaintenanceRecord>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var items = _store.MaintenanceRecords
            .Where(x => x.VehicleId == vehicleId)
            .OrderByDescending(x => x.PerformedAtUtc)
            .ToList();

        return Task.FromResult<IReadOnlyList<MaintenanceRecord>>(items);
    }
}
