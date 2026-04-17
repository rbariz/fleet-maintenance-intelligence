namespace FleetMaintenanceIntelligence.Contracts.Vehicles
{
    public sealed record VehicleTimelineItemResponse(
    DateTime OccurredAtUtc,
    string ItemType,
    string Title,
    string Description,
    string Severity
);
}
