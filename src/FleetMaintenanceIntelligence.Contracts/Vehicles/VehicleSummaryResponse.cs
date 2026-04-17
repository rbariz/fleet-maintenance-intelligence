namespace FleetMaintenanceIntelligence.Contracts.Vehicles
{
    public sealed record VehicleSummaryResponse(
        Guid Id,
        string RegistrationNumber,
        string Vin,
        string Brand,
        string Model,
        int Year,
        int CurrentMileageKm,
        string Status,
        IReadOnlyList<VehicleMaintenancePlanSummaryResponse> MaintenancePlans,
        VehicleTelemetrySummaryResponse? LatestTelemetry,
        IReadOnlyList<VehicleOpenAlertSummaryResponse> OpenAlerts
    );
}
