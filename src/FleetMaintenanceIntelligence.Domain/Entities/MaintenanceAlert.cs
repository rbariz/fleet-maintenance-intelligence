using FleetMaintenanceIntelligence.Domain.Enums;
using FleetMaintenanceIntelligence.Domain.Exceptions;

namespace FleetMaintenanceIntelligence.Domain.Entities
{
    public sealed class MaintenanceAlert
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public AlertSeverity Severity { get; private set; }
        public AlertStatus Status { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? ResolvedAtUtc { get; private set; }

        private MaintenanceAlert() { }

        public MaintenanceAlert(
            Guid id,
            Guid vehicleId,
            string title,
            string description,
            AlertSeverity severity,
            DateTime createdAtUtc)
        {
            if (vehicleId == Guid.Empty)
                throw new DomainException("VehicleId is required.");

            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title is required.");

            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description is required.");

            Id = id;
            VehicleId = vehicleId;
            Title = title;
            Description = description;
            Severity = severity;
            CreatedAtUtc = createdAtUtc;
            Status = AlertStatus.Open;
        }

        public void Acknowledge()
        {
            if (Status != AlertStatus.Open)
                throw new DomainException("Only open alerts can be acknowledged.");

            Status = AlertStatus.Acknowledged;
        }

        public void Resolve(DateTime nowUtc)
        {
            if (Status == AlertStatus.Resolved)
                throw new DomainException("Alert is already resolved.");

            Status = AlertStatus.Resolved;
            ResolvedAtUtc = nowUtc;
        }
    }
}
