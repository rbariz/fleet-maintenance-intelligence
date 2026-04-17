using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Infrastructure.Persistence;

public sealed class InMemoryVehicleTelemetrySnapshotRepository : IVehicleTelemetrySnapshotRepository
{
    private readonly InMemoryStore _store;

    public InMemoryVehicleTelemetrySnapshotRepository(InMemoryStore store)
    {
        _store = store;
    }

    public Task<IReadOnlyList<VehicleTelemetrySnapshot>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var items = _store.TelemetrySnapshots
            .Where(x => x.VehicleId == vehicleId)
            .OrderByDescending(x => x.RecordedAtUtc)
            .ToList();

        return Task.FromResult<IReadOnlyList<VehicleTelemetrySnapshot>>(items);
    }

    public Task<VehicleTelemetrySnapshot?> GetLatestByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var item = _store.TelemetrySnapshots
            .Where(x => x.VehicleId == vehicleId)
            .OrderByDescending(x => x.RecordedAtUtc)
            .FirstOrDefault();

        return Task.FromResult(item);
    }
}
