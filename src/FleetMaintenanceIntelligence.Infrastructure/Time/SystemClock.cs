using FleetMaintenanceIntelligence.Application.Abstractions;

namespace FleetMaintenanceIntelligence.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
