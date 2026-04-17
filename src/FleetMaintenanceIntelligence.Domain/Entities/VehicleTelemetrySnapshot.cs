using FleetMaintenanceIntelligence.Domain.Exceptions;

namespace FleetMaintenanceIntelligence.Domain.Entities
{
    public sealed class VehicleTelemetrySnapshot
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public int MileageKm { get; private set; }
        public decimal FuelLevelPercent { get; private set; }
        public decimal BatteryVoltage { get; private set; }
        public decimal EngineTemperatureCelsius { get; private set; }
        public DateTime RecordedAtUtc { get; private set; }

        private VehicleTelemetrySnapshot() { }

        public VehicleTelemetrySnapshot(
            Guid id,
            Guid vehicleId,
            int mileageKm,
            decimal fuelLevelPercent,
            decimal batteryVoltage,
            decimal engineTemperatureCelsius,
            DateTime recordedAtUtc)
        {
            if (vehicleId == Guid.Empty)
                throw new DomainException("VehicleId is required.");

            if (mileageKm < 0)
                throw new DomainException("Mileage cannot be negative.");

            if (fuelLevelPercent < 0 || fuelLevelPercent > 100)
                throw new DomainException("Fuel level must be between 0 and 100.");

            if (batteryVoltage < 0)
                throw new DomainException("Battery voltage cannot be negative.");

            Id = id;
            VehicleId = vehicleId;
            MileageKm = mileageKm;
            FuelLevelPercent = fuelLevelPercent;
            BatteryVoltage = batteryVoltage;
            EngineTemperatureCelsius = engineTemperatureCelsius;
            RecordedAtUtc = recordedAtUtc;
        }
    }
}
