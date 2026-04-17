namespace FleetMaintenanceIntelligence.Contracts.Vehicles
{
    public sealed record VehicleOpenAlertSummaryResponse(
        Guid Id,
        string Title,
        string Description,
        string Severity,
        string Status,
        DateTime CreatedAtUtc
    );
}
