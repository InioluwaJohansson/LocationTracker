using LocationTracker.Authentication;
using LocationTracker.Context;
using Microsoft.EntityFrameworkCore;
namespace LocationTracker.BackgroundServices;
public class StartupInitializer : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public StartupInitializer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LocationTrackerContext>();
        await context.Database.MigrateAsync();
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
public class LocationTrackerServices : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public LocationTrackerServices(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var authControl = scope.ServiceProvider.GetRequiredService<IJWTAuthentication>();
                await authControl.GetSigningData();
                await Task.Delay(TimeSpan.FromSeconds(9), stoppingToken);
                await authControl.RefreshAllTokens();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Background service error: {ex.Message}");
            }
            await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
        }
    }
}