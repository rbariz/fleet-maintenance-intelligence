using FleetMaintenanceIntelligence.Application.UseCases.EvaluateMaintenanceAlerts;
using FleetMaintenanceIntelligence.Application.UseCases.GetFleetDashboard;
using FleetMaintenanceIntelligence.Application.UseCases.GetMaintenanceAlerts;
using FleetMaintenanceIntelligence.Application.UseCases.GetVehicleTimeline;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetMaintenanceIntelligence.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<EvaluateMaintenanceAlertsHandler>();
            services.AddScoped<GetMaintenanceAlertsHandler>();
            services.AddScoped<GetVehicleTimelineHandler>();
            services.AddScoped<GetFleetDashboardHandler>();
            return services;
        }
    }
}
