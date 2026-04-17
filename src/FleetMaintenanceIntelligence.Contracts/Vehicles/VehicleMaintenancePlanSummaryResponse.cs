namespace FleetMaintenanceIntelligence.Contracts.Vehicles
{
    public sealed record VehicleMaintenancePlanSummaryResponse(
    Guid Id,
    string Name,
    string PlanType,
    int? EveryKm,
    int? EveryDays,
    int LastServiceMileageKm,
    DateTime? LastServiceDateUtc,
    bool IsActive
);


}
