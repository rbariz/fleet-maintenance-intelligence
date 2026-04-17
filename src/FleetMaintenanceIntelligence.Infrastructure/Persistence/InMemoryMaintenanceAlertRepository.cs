using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Domain.Entities;
using FleetMaintenanceIntelligence.Domain.Enums;

namespace FleetMaintenanceIntelligence.Infrastructure.Persistence;

public sealed class InMemoryMaintenanceAlertRepository : IMaintenanceAlertRepository
{
    private readonly InMemoryStore _store;

    public InMemoryMaintenanceAlertRepository(InMemoryStore store)
    {
        _store = store;
    }

    public Task AddAsync(MaintenanceAlert alert, CancellationToken cancellationToken = default)
    {
        _store.MaintenanceAlerts.Add(alert);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<MaintenanceAlert>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = _store.MaintenanceAlerts
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToList();

        return Task.FromResult<IReadOnlyList<MaintenanceAlert>>(items);
    }

    public Task<bool> ExistsOpenAlertAsync(
    Guid vehicleId,
    string title,
    CancellationToken cancellationToken = default)
    {
        var exists = _store.MaintenanceAlerts.Any(x =>
            x.VehicleId == vehicleId &&
            x.Title == title &&
            (x.Status == AlertStatus.Open || x.Status == AlertStatus.Acknowledged));

        return Task.FromResult(exists);
    }
}
