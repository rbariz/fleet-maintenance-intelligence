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
