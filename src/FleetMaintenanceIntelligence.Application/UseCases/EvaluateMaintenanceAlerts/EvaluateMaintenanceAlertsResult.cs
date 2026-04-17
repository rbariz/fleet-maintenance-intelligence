namespace FleetMaintenanceIntelligence.Application.UseCases.EvaluateMaintenanceAlerts
{
    public sealed record EvaluateMaintenanceAlertsResult(
    Guid VehicleId,
    int CreatedAlertsCount,
    IReadOnlyList<string> CreatedAlertTitles
);
}
