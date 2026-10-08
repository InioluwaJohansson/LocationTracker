using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using LocationTracker.Authentication;
namespace LocationTracker.RealTime.Hubs;
public class LocationTrackerHub : Hub
{
    IJWTAuthentication jWTAuthentication;
    public LocationTrackerHub(IJWTAuthentication jwtAuthentication)
    {
        jWTAuthentication = jwtAuthentication;
    }
    private static readonly Dictionary<int, HashSet<string>> _connections = new();
    private int GetPersonId()
    {
        var token = Context.GetHttpContext()?.Request.Query["access_token"].FirstOrDefault();
        var user = jWTAuthentication.GetUserFromToken(token ?? string.Empty);
        if (user != null) return user.PersonId;
        else throw new HubException("Invalid token or user not found.");
        
    }
    public override async Task OnConnectedAsync()
    {
        var personId = GetPersonId();
        var connectionId = Context.ConnectionId;
        lock (_connections)
        {
            if (!_connections.ContainsKey(personId))
                _connections[personId] = new HashSet<string>();

            _connections[personId].Add(connectionId);
        }
        await Clients.All.SendAsync("UserOnline", personId);
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var personId = GetPersonId();
        var connectionId = Context.ConnectionId;
        bool shouldMarkOffline = false;
        lock (_connections)
        {
            if (_connections.ContainsKey(personId))
            {
                _connections[personId].Remove(connectionId);
                if (_connections[personId].Count == 0)
                {
                    _connections.Remove(personId);
                    shouldMarkOffline = true;
                }
            }
        }
        if (shouldMarkOffline)
        {
            await Clients.All.SendAsync("UserOffline", personId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}