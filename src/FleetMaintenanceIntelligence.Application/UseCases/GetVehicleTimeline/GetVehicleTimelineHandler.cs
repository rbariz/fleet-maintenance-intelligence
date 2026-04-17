using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Contracts.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetMaintenanceIntelligence.Application.UseCases.GetVehicleTimeline
{
    public sealed class GetVehicleTimelineHandler
    {
        private readonly IFleetVehicleRepository _vehicleRepository;
        private readonly IMaintenanceRecordRepository _maintenanceRecordRepository;
        private readonly IVehicleTelemetrySnapshotRepository _telemetryRepository;
        private readonly IMaintenanceAlertRepository _alertRepository;

        public GetVehicleTimelineHandler(
            IFleetVehicleRepository vehicleRepository,
            IMaintenanceRecordRepository maintenanceRecordRepository,
            IVehicleTelemetrySnapshotRepository telemetryRepository,
            IMaintenanceAlertRepository alertRepository)
        {
            _vehicleRepository = vehicleRepository;
            _maintenanceRecordRepository = maintenanceRecordRepository;
            _telemetryRepository = telemetryRepository;
            _alertRepository = alertRepository;
        }

        public async Task<IReadOnlyList<VehicleTimelineItemResponse>> HandleAsync(
            Guid vehicleId,
            CancellationToken cancellationToken = default)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);
            if (vehicle is null)
                return [];

            var timeline = new List<VehicleTimelineItemResponse>();

            var records = await _maintenanceRecordRepository.GetByVehicleIdAsync(vehicleId, cancellationToken);
            timeline.AddRange(records.Select(x => new VehicleTimelineItemResponse(
                x.PerformedAtUtc,
                "maintenance_record",
                x.RecordType.ToString(),
                $"{x.Description} | Mileage: {x.MileageKm} km | Cost: {x.CostAmount} {x.Currency}",
                "info"
            )));

            var snapshots = await _telemetryRepository.GetByVehicleIdAsync(vehicleId, cancellationToken);
            timeline.AddRange(snapshots.Select(x => new VehicleTimelineItemResponse(
                x.RecordedAtUtc,
                "telemetry_snapshot",
                "Telemetry Snapshot",
                $"Mileage: {x.MileageKm} km | Fuel: {x.FuelLevelPercent}% | Battery: {x.BatteryVoltage}V | Temp: {x.EngineTemperatureCelsius}°C",
                "info"
            )));

            var alerts = await _alertRepository.GetByVehicleIdAsync(vehicleId, cancellationToken);
            timeline.AddRange(alerts.Select(x => new VehicleTimelineItemResponse(
                x.CreatedAtUtc,
                "maintenance_alert",
                x.Title,
                x.Description,
                x.Severity.ToString().ToLowerInvariant()
            )));

            return timeline
                .OrderByDescending(x => x.OccurredAtUtc)
                .ToList();
        }
    }
}
