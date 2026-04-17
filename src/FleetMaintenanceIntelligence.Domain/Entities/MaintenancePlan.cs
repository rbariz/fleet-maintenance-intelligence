using FleetMaintenanceIntelligence.Domain.Enums;
using FleetMaintenanceIntelligence.Domain.Exceptions;

namespace FleetMaintenanceIntelligence.Domain.Entities
{
    public sealed class MaintenancePlan
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public string Name { get; private set; }
        public MaintenancePlanType PlanType { get; private set; }
        public int? EveryKm { get; private set; }
        public int? EveryDays { get; private set; }
        public int LastServiceMileageKm { get; private set; }
        public DateTime? LastServiceDateUtc { get; private set; }
        public bool IsActive { get; private set; }

        private MaintenancePlan() { }

        public MaintenancePlan(
            Guid id,
            Guid vehicleId,
            string name,
            MaintenancePlanType planType,
            int? everyKm,
            int? everyDays,
            int lastServiceMileageKm,
            DateTime? lastServiceDateUtc)
        {
            if (vehicleId == Guid.Empty)
                throw new DomainException("VehicleId is required.");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Plan name is required.");

            if (lastServiceMileageKm < 0)
                throw new DomainException("Last service mileage cannot be negative.");

            if (planType == MaintenancePlanType.DistanceBased && (!everyKm.HasValue || everyKm <= 0))
                throw new DomainException("Distance-based plan requires EveryKm.");

            if (planType == MaintenancePlanType.TimeBased && (!everyDays.HasValue || everyDays <= 0))
                throw new DomainException("Time-based plan requires EveryDays.");

            if (planType == MaintenancePlanType.Hybrid &&
                ((!everyKm.HasValue || everyKm <= 0) || (!everyDays.HasValue || everyDays <= 0)))
                throw new DomainException("Hybrid plan requires EveryKm and EveryDays.");

            Id = id;
            VehicleId = vehicleId;
            Name = name;
            PlanType = planType;
            EveryKm = everyKm;
            EveryDays = everyDays;
            LastServiceMileageKm = lastServiceMileageKm;
            LastServiceDateUtc = lastServiceDateUtc;
            IsActive = true;
        }

        public void MarkServiced(int mileageKm, DateTime servicedAtUtc)
        {
            if (mileageKm < 0)
                throw new DomainException("Mileage cannot be negative.");

            LastServiceMileageKm = mileageKm;
            LastServiceDateUtc = servicedAtUtc;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}
