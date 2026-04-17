using FleetMaintenanceIntelligence.Application.DependencyInjection;
using FleetMaintenanceIntelligence.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

app.MapGet("/fleet-dashboard", context =>
{
    context.Response.Redirect("/fleet-dashboard.html");
    return Task.CompletedTask;
});

app.Services.SeedInMemoryData();

app.Run();