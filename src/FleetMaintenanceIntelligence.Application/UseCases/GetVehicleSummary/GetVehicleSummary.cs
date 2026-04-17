using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Contracts.Vehicles;
using FleetMaintenanceIntelligence.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetMaintenanceIntelligence.Application.UseCases.GetVehicleSummary
{
    public sealed class GetVehicleSummaryHandler
    {
        private readonly IFleetVehicleRepository _vehicleRepository;
        private readonly IMaintenancePlanRepository _maintenancePlanRepository;
        private readonly IVehicleTelemetrySnapshotRepository _telemetryRepository;
        private readonly IMaintenanceAlertRepository _alertRepository;

        public GetVehicleSummaryHandler(
            IFleetVehicleRepository vehicleRepository,
            IMaintenancePlanRepository maintenancePlanRepository,
            IVehicleTelemetrySnapshotRepository telemetryRepository,
            IMaintenanceAlertRepository alertRepository)
        {
            _vehicleRepository = vehicleRepository;
            _maintenancePlanRepository = maintenancePlanRepository;
            _telemetryRepository = telemetryRepository;
            _alertRepository = alertRepository;
        }

        public async Task<VehicleSummaryResponse?> HandleAsync(
            Guid vehicleId,
            CancellationToken cancellationToken = default)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);
            if (vehicle is null)
                return null;

            var plans = await _maintenancePlanRepository.GetByVehicleIdAsync(vehicleId, cancellationToken);
            var latestTelemetry = await _telemetryRepository.GetLatestByVehicleIdAsync(vehicleId, cancellationToken);
            var alerts = await _alertRepository.GetByVehicleIdAsync(vehicleId, cancellationToken);

            var openAlerts = alerts
                .Where(x => x.Status == AlertStatus.Open || x.Status == AlertStatus.Acknowledged)
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(x => new VehicleOpenAlertSummaryResponse(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.Severity.ToString().ToLowerInvariant(),
                    x.Status.ToString().ToLowerInvariant(),
                    x.CreatedAtUtc))
                .ToList();

            var maintenancePlans = plans
                .OrderBy(x => x.Name)
                .Select(x => new VehicleMaintenancePlanSummaryResponse(
                    x.Id,
                    x.Name,
                    x.PlanType.ToString().ToLowerInvariant(),
                    x.EveryKm,
                    x.EveryDays,
                    x.LastServiceMileageKm,
                    x.LastServiceDateUtc,
                    x.IsActive))
                .ToList();

            var telemetry = latestTelemetry is null
                ? null
                : new VehicleTelemetrySummaryResponse(
                    latestTelemetry.MileageKm,
                    latestTelemetry.FuelLevelPercent,
                    latestTelemetry.BatteryVoltage,
                    latestTelemetry.EngineTemperatureCelsius,
                    latestTelemetry.RecordedAtUtc);

            return new VehicleSummaryResponse(
                vehicle.Id,
                vehicle.RegistrationNumber,
                vehicle.Vin,
                vehicle.Brand,
                vehicle.Model,
                vehicle.Year,
                vehicle.CurrentMileageKm,
                vehicle.Status.ToString().ToLowerInvariant(),
                maintenancePlans,
                telemetry,
                openAlerts);
        }
    }
}
