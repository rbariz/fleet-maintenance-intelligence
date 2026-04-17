using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Domain.Entities;
using FleetMaintenanceIntelligence.Domain.Enums;
using FleetMaintenanceIntelligence.Domain.Exceptions;

namespace FleetMaintenanceIntelligence.Application.UseCases.EvaluateMaintenanceAlerts
{
    public sealed class EvaluateMaintenanceAlertsHandler
    {
        private readonly IFleetVehicleRepository _fleetVehicleRepository;
        private readonly IMaintenancePlanRepository _maintenancePlanRepository;
        private readonly IVehicleTelemetrySnapshotRepository _telemetryRepository;
        private readonly IMaintenanceAlertRepository _alertRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public EvaluateMaintenanceAlertsHandler(
            IFleetVehicleRepository fleetVehicleRepository,
            IMaintenancePlanRepository maintenancePlanRepository,
            IVehicleTelemetrySnapshotRepository telemetryRepository,
            IMaintenanceAlertRepository alertRepository,
            IUnitOfWork unitOfWork,
            IClock clock)
        {
            _fleetVehicleRepository = fleetVehicleRepository;
            _maintenancePlanRepository = maintenancePlanRepository;
            _telemetryRepository = telemetryRepository;
            _alertRepository = alertRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        public async Task<EvaluateMaintenanceAlertsResult> HandleAsync(
            EvaluateMaintenanceAlertsRequest request,
            CancellationToken cancellationToken = default)
        {
            var vehicle = await _fleetVehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
            if (vehicle is null)
                throw new DomainException("Vehicle not found.");

            var nowUtc = _clock.UtcNow;
            var createdTitles = new List<string>();

            var plans = await _maintenancePlanRepository.GetByVehicleIdAsync(vehicle.Id, cancellationToken);

            //foreach (var plan in plans.Where(x => x.IsActive))
            //{
            //    EvaluatePlan(vehicle, plan, nowUtc, createdTitles, cancellationToken).GetAwaiter().GetResult();
            //}

            foreach (var plan in plans.Where(x => x.IsActive))
            {
                await EvaluatePlan(vehicle, plan, nowUtc, createdTitles, cancellationToken);
            }

            var snapshot = await _telemetryRepository.GetLatestByVehicleIdAsync(vehicle.Id, cancellationToken);
            if (snapshot is not null)
            {
                await EvaluateTelemetryAsync(vehicle, snapshot, nowUtc, createdTitles, cancellationToken);
            }

            if (createdTitles.Count > 0)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return new EvaluateMaintenanceAlertsResult(
                vehicle.Id,
                createdTitles.Count,
                createdTitles);
        }

        private async Task EvaluatePlan(
            FleetVehicle vehicle,
            MaintenancePlan plan,
            DateTime nowUtc,
            List<string> createdTitles,
            CancellationToken cancellationToken)
        {
            var dueSoon = false;
            var overdue = false;

            if (plan.PlanType is MaintenancePlanType.DistanceBased or MaintenancePlanType.Hybrid)
            {
                if (plan.EveryKm.HasValue)
                {
                    var nextDueMileage = plan.LastServiceMileageKm + plan.EveryKm.Value;

                    if (vehicle.CurrentMileageKm >= nextDueMileage)
                        overdue = true;
                    else if (vehicle.CurrentMileageKm >= nextDueMileage - 500)
                        dueSoon = true;
                }
            }

            if (plan.PlanType is MaintenancePlanType.TimeBased or MaintenancePlanType.Hybrid)
            {
                if (plan.EveryDays.HasValue && plan.LastServiceDateUtc.HasValue)
                {
                    var dueDate = plan.LastServiceDateUtc.Value.AddDays(plan.EveryDays.Value);

                    if (nowUtc >= dueDate)
                        overdue = true;
                    else if (nowUtc >= dueDate.AddDays(-15))
                        dueSoon = true;
                }
            }

            if (overdue)
            {
                var title = $"Maintenance overdue: {plan.Name}";
                await CreateAlertAsync(
                    vehicle.Id,
                    title,
                    $"Vehicle {vehicle.RegistrationNumber} is overdue for maintenance plan '{plan.Name}'.",
                    AlertSeverity.High,
                    nowUtc,
                    createdTitles,
                    cancellationToken);
            }
            else if (dueSoon)
            {
                var title = $"Maintenance due soon: {plan.Name}";
                await CreateAlertAsync(
                    vehicle.Id,
                    title,
                    $"Vehicle {vehicle.RegistrationNumber} is approaching maintenance plan '{plan.Name}'.",
                    AlertSeverity.Medium,
                    nowUtc,
                    createdTitles,
                    cancellationToken);
            }
        }

        private async Task EvaluateTelemetryAsync(
            FleetVehicle vehicle,
            VehicleTelemetrySnapshot snapshot,
            DateTime nowUtc,
            List<string> createdTitles,
            CancellationToken cancellationToken)
        {
            if (snapshot.EngineTemperatureCelsius >= 110)
            {
                await CreateAlertAsync(
                    vehicle.Id,
                    "Critical engine temperature",
                    $"Vehicle {vehicle.RegistrationNumber} has engine temperature at {snapshot.EngineTemperatureCelsius}°C.",
                    AlertSeverity.Critical,
                    nowUtc,
                    createdTitles,
                    cancellationToken);
            }

            if (snapshot.BatteryVoltage < 11.8m)
            {
                await CreateAlertAsync(
                    vehicle.Id,
                    "Low battery voltage",
                    $"Vehicle {vehicle.RegistrationNumber} battery voltage is low at {snapshot.BatteryVoltage}V.",
                    AlertSeverity.High,
                    nowUtc,
                    createdTitles,
                    cancellationToken);
            }

            if (snapshot.FuelLevelPercent < 10m)
            {
                await CreateAlertAsync(
                    vehicle.Id,
                    "Low fuel level",
                    $"Vehicle {vehicle.RegistrationNumber} fuel level is low at {snapshot.FuelLevelPercent}%.",
                    AlertSeverity.Low,
                    nowUtc,
                    createdTitles,
                    cancellationToken);
            }
        }

        private async Task CreateAlertAsync(
            Guid vehicleId,
            string title,
            string description,
            AlertSeverity severity,
            DateTime nowUtc,
            List<string> createdTitles,
            CancellationToken cancellationToken)
        {
            var alert = new MaintenanceAlert(
                Guid.NewGuid(),
                vehicleId,
                title,
                description,
                severity,
                nowUtc);

            await _alertRepository.AddAsync(alert, cancellationToken);
            createdTitles.Add(title);
        }
    }
}
