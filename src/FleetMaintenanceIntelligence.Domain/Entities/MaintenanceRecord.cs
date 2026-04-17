using FleetMaintenanceIntelligence.Domain.Enums;
using FleetMaintenanceIntelligence.Domain.Exceptions;

namespace FleetMaintenanceIntelligence.Domain.Entities
{
    public sealed class MaintenanceRecord
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid? MaintenancePlanId { get; private set; }
        public MaintenanceRecordType RecordType { get; private set; }
        public string Description { get; private set; }
        public int MileageKm { get; private set; }
        public decimal CostAmount { get; private set; }
        public string Currency { get; private set; }
        public DateTime PerformedAtUtc { get; private set; }

        private MaintenanceRecord() { }

        public MaintenanceRecord(
            Guid id,
            Guid vehicleId,
            Guid? maintenancePlanId,
            MaintenanceRecordType recordType,
            string description,
            int mileageKm,
            decimal costAmount,
            string currency,
            DateTime performedAtUtc)
        {
            if (vehicleId == Guid.Empty)
                throw new DomainException("VehicleId is required.");

            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description is required.");

            if (mileageKm < 0)
                throw new DomainException("Mileage cannot be negative.");

            if (costAmount < 0)
                throw new DomainException("Cost cannot be negative.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainException("Currency is required.");

            Id = id;
            VehicleId = vehicleId;
            MaintenancePlanId = maintenancePlanId;
            RecordType = recordType;
            Description = description;
            MileageKm = mileageKm;
            CostAmount = costAmount;
            Currency = currency;
            PerformedAtUtc = performedAtUtc;
        }
    }
}
