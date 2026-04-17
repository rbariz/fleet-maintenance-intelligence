namespace FleetMaintenanceIntelligence.Contracts.Vehicles
{
    public sealed record VehicleTelemetrySummaryResponse(
    int MileageKm,
    decimal FuelLevelPercent,
    decimal BatteryVoltage,
    decimal EngineTemperatureCelsius,
    DateTime RecordedAtUtc
);


}
