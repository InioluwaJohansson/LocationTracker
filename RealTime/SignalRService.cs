using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using LocationTracker.RealTime.Hubs;
using Home_Security.RealTimeServices;
namespace LocationTracker.RealTimeServices;

[Authorize]
public class RealtimeNotificationService : IRealtimeNotificationService
{
    private readonly IHubContext<LocationTrackerHub> _hub;
    public RealtimeNotificationService(IHubContext<LocationTrackerHub> hub)
    {
        _hub = hub;
    }
    public async Task NotifyAll<T>(string eventName, T payload)
    {
        await _hub.Clients.All.SendAsync(eventName, payload);
    }
}
