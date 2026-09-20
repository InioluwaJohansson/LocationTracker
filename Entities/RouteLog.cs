using LocationTracker.Contracts;
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
    public string TravelMode { get; set; } = "vehicle";
    public DateTimeOffset CompletedAt { get; set; } = DateTimeOffset.UtcNow;
}