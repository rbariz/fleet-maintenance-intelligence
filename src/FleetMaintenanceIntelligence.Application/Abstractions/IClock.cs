namespace FleetMaintenanceIntelligence.Application.Abstractions
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
