namespace FleetMaintenanceIntelligence.Contracts.MaintenanceAlerts
{
    public sealed record EvaluateMaintenanceAlertsResponse(
    Guid VehicleId,
    int CreatedAlertsCount,
    IReadOnlyList<string> CreatedAlertTitles
);
}
