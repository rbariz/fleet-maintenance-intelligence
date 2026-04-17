param(
    [string]$Root = "D:\SAASs\fleet-maintenance-intelligence\src\FleetMaintenanceIntelligence.Infrastructure"
)

function Write-CodeFile {
    param(
        [string]$Path,
        [string]$Content
    )

    $directory = Split-Path $Path -Parent
    New-Item -ItemType Directory -Path $directory -Force | Out-Null
    Set-Content -Path $Path -Value $Content -Encoding UTF8
}

$PersistencePath = Join-Path $Root "Persistence"
$DependencyInjectionPath = Join-Path $Root "DependencyInjection"
$TimePath = Join-Path $Root "Time"

# =========================
# InMemoryStore
# =========================
Write-CodeFile -Path (Join-Path $PersistencePath "InMemoryStore.cs") -Content @'
using FleetMaintenanceIntelligence.Domain.Entities;

namespace FleetMaintenanceIntelligence.Infrastructure.Persistence;

public sealed class InMemoryStore
{
    public List<FleetVehicle> Vehicles { get; } = [];
    public List<MaintenancePlan> MaintenancePlans { get; } = [];
    public List<MaintenanceRecord> MaintenanceRecords { get; } = [];
    public List<VehicleTelemetrySnapshot> TelemetrySnapshots { get; } = [];
    public List<MaintenanceAlert> MaintenanceAlerts { get; } = [];
}
'@

# =========================
# FleetVehicleRepository
# =========================
Write-CodeFile -Path (Join-Path $PersistencePath "InMemoryFleetVehicleRepository.cs") -Content @'
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
'@

# =========================
# MaintenancePlanRepository
# =========================
Write-CodeFile -Path (Join-Path $PersistencePath "InMemoryMaintenancePlanRepository.cs") -Content @'
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
'@

# =========================
# Telemetry Repository
# =========================
Write-CodeFile -Path (Join-Path $PersistencePath "InMemoryVehicleTelemetrySnapshotRepository.cs") -Content @'
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

    public Task<VehicleTelemetrySnapshot?> GetLatestByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var item = _store.TelemetrySnapshots
            .Where(x => x.VehicleId == vehicleId)
            .OrderByDescending(x => x.RecordedAtUtc)
            .FirstOrDefault();

        return Task.FromResult(item);
    }
}
'@

# =========================
# Alert Repository
# =========================
Write-CodeFile -Path (Join-Path $PersistencePath "InMemoryMaintenanceAlertRepository.cs") -Content @'
using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Domain.Entities;

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
}
'@

# =========================
# UnitOfWork
# =========================
Write-CodeFile -Path (Join-Path $PersistencePath "InMemoryUnitOfWork.cs") -Content @'
using FleetMaintenanceIntelligence.Application.Abstractions;

namespace FleetMaintenanceIntelligence.Infrastructure.Persistence;

public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
'@

# =========================
# Clock
# =========================
Write-CodeFile -Path (Join-Path $TimePath "SystemClock.cs") -Content @'
using FleetMaintenanceIntelligence.Application.Abstractions;

namespace FleetMaintenanceIntelligence.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
'@

# =========================
# Seed
# =========================
Write-CodeFile -Path (Join-Path $DependencyInjectionPath "InMemorySeed.cs") -Content @'
using FleetMaintenanceIntelligence.Domain.Entities;
using FleetMaintenanceIntelligence.Domain.Enums;
using FleetMaintenanceIntelligence.Infrastructure.Persistence;

namespace FleetMaintenanceIntelligence.Infrastructure.DependencyInjection;

internal static class InMemorySeed
{
    public static readonly Guid DemoVehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static void Seed(InMemoryStore store)
    {
        if (store.Vehicles.Count > 0)
            return;

        var now = DateTime.UtcNow;

        var vehicle = new FleetVehicle(
            DemoVehicleId,
            "12345-A-7",
            "VIN-DEMO-123",
            "Toyota",
            "Hilux",
            2022,
            10450,
            now.AddMonths(-6));

        var maintenancePlan1 = new MaintenancePlan(
            Guid.NewGuid(),
            vehicle.Id,
            "Oil Change",
            MaintenancePlanType.DistanceBased,
            10000,
            null,
            0,
            now.AddMonths(-4));

        var maintenancePlan2 = new MaintenancePlan(
            Guid.NewGuid(),
            vehicle.Id,
            "Inspection",
            MaintenancePlanType.TimeBased,
            null,
            90,
            8000,
            now.AddDays(-100));

        var snapshot = new VehicleTelemetrySnapshot(
            Guid.NewGuid(),
            vehicle.Id,
            10450,
            8,
            11.6m,
            112,
            now);

        store.Vehicles.Add(vehicle);
        store.MaintenancePlans.Add(maintenancePlan1);
        store.MaintenancePlans.Add(maintenancePlan2);
        store.TelemetrySnapshots.Add(snapshot);
    }
}
'@

# =========================
# DI
# =========================
Write-CodeFile -Path (Join-Path $DependencyInjectionPath "ServiceCollectionExtensions.cs") -Content @'
using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Infrastructure.Persistence;
using FleetMaintenanceIntelligence.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace FleetMaintenanceIntelligence.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryStore>();

        services.AddScoped<IFleetVehicleRepository, InMemoryFleetVehicleRepository>();
        services.AddScoped<IMaintenancePlanRepository, InMemoryMaintenancePlanRepository>();
        services.AddScoped<IVehicleTelemetrySnapshotRepository, InMemoryVehicleTelemetrySnapshotRepository>();
        services.AddScoped<IMaintenanceAlertRepository, InMemoryMaintenanceAlertRepository>();
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }

    public static void SeedInMemoryData(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<InMemoryStore>();
        InMemorySeed.Seed(store);
    }
}
'@

Write-Host "Infrastructure scaffold FIXED and generated successfully." -ForegroundColor Green