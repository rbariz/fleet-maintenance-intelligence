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
