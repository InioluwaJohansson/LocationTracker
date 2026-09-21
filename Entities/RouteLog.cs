using LocationTracker.Contracts;
using LocationTracker.Models.Enums;
namespace LocationTracker.Entities;
public class CompletedRouteLog : AuditableEntity
{
    public int UserId { get; set; }
    public int RouteId { get; set; }
    public int JourneyId { get; set; }
    public string OriginName { get; set; } = string.Empty;
    public string DestinationName { get; set; } = string.Empty;
    public float TotalDistanceMeters { get; set; }
    public int DurationSeconds { get; set; }
    public TravelMode TravelMode { get; set; } = TravelMode.Vehicle;
    public DateTimeOffset CompletedAt { get; set; } = DateTimeOffset.UtcNow;
}