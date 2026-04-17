using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Infrastructure.Persistence;
using FleetMaintenanceIntelligence.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace FleetMaintenanceIntelligence.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryStore>();

        services.AddScoped<IFleetVehicleRepository, InMemoryFleetVehicleRepository>();
        services.AddScoped<IMaintenancePlanRepository, InMemoryMaintenancePlanRepository>();
        services.AddScoped<IVehicleTelemetrySnapshotRepository, InMemoryVehicleTelemetrySnapshotRepository>();
        services.AddScoped<IMaintenanceAlertRepository, InMemoryMaintenanceAlertRepository>();
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }

    public static void SeedInMemoryData(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<InMemoryStore>();
        InMemorySeed.Seed(store);
    }
}
