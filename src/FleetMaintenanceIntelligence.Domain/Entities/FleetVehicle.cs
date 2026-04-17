using FleetMaintenanceIntelligence.Domain.Enums;
using FleetMaintenanceIntelligence.Domain.Exceptions;

namespace FleetMaintenanceIntelligence.Domain.Entities
{
    public sealed class FleetVehicle
    {
        public Guid Id { get; private set; }
        public string RegistrationNumber { get; private set; }
        public string Vin { get; private set; }
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public int Year { get; private set; }
        public int CurrentMileageKm { get; private set; }
        public VehicleStatus Status { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? UpdatedAtUtc { get; private set; }

        private FleetVehicle() { }

        public FleetVehicle(
            Guid id,
            string registrationNumber,
            string vin,
            string brand,
            string model,
            int year,
            int currentMileageKm,
            DateTime createdAtUtc)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber))
                throw new DomainException("Registration number is required.");

            if (string.IsNullOrWhiteSpace(vin))
                throw new DomainException("VIN is required.");

            if (string.IsNullOrWhiteSpace(brand))
                throw new DomainException("Brand is required.");

            if (string.IsNullOrWhiteSpace(model))
                throw new DomainException("Model is required.");

            if (year < 1980 || year > DateTime.UtcNow.Year + 1)
                throw new DomainException("Vehicle year is invalid.");

            if (currentMileageKm < 0)
                throw new DomainException("Current mileage cannot be negative.");

            Id = id;
            RegistrationNumber = registrationNumber;
            Vin = vin;
            Brand = brand;
            Model = model;
            Year = year;
            CurrentMileageKm = currentMileageKm;
            CreatedAtUtc = createdAtUtc;
            Status = VehicleStatus.Active;
        }

        public void UpdateMileage(int mileageKm, DateTime nowUtc)
        {
            if (mileageKm < CurrentMileageKm)
                throw new DomainException("Mileage cannot decrease.");

            CurrentMileageKm = mileageKm;
            UpdatedAtUtc = nowUtc;
        }

        public void PutInMaintenance(DateTime nowUtc)
        {
            if (Status == VehicleStatus.Retired)
                throw new DomainException("Retired vehicles cannot enter maintenance.");

            Status = VehicleStatus.InMaintenance;
            UpdatedAtUtc = nowUtc;
        }

        public void MarkActive(DateTime nowUtc)
        {
            if (Status == VehicleStatus.Retired)
                throw new DomainException("Retired vehicles cannot become active.");

            Status = VehicleStatus.Active;
            UpdatedAtUtc = nowUtc;
        }

        public void MarkOutOfService(DateTime nowUtc)
        {
            if (Status == VehicleStatus.Retired)
                throw new DomainException("Retired vehicles cannot change status.");

            Status = VehicleStatus.OutOfService;
            UpdatedAtUtc = nowUtc;
        }

        public void Retire(DateTime nowUtc)
        {
            Status = VehicleStatus.Retired;
            UpdatedAtUtc = nowUtc;
        }
    }
}
