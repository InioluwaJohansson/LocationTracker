using LocationTracker.Models.Enums;

namespace LocationTracker.Models.DTOs;
public class CreateRouteLogDto
{
    public int UserId { get; set; }
    public int RouteId { get; set; }
    public int JourneyId { get; set; }
    public string OriginName { get; set; } = string.Empty;
    public string DestinationName { get; set; } = string.Empty;
    public float TotalDistanceMeters { get; set; }
    public int DurationSeconds { get; set; }
    public TravelMode TravelMode { get; set; }
    public DateTimeOffset CompletedAt { get; set; } = DateTimeOffset.UtcNow;
}
public class GetRouteLogDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RouteId { get; set; }
    public int JourneyId { get; set; }
    public string OriginName { get; set; } = string.Empty;
    public string DestinationName { get; set; } = string.Empty;
    public float TotalDistanceMeters { get; set; }
    public int DurationSeconds { get; set; }
    public TravelMode TravelMode { get; set; }
    public DateTimeOffset CompletedAt { get; set; } = DateTimeOffset.UtcNow;
}
public class UpdateRouteLogDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RouteId { get; set; }
    public int JourneyId { get; set; }
    public string OriginName { get; set; } = string.Empty;
    public string DestinationName { get; set; } = string.Empty;
    public float TotalDistanceMeters { get; set; }
    public int DurationSeconds { get; set; }
    public TravelMode TravelMode { get; set; }
    public DateTimeOffset CompletedAt { get; set; } = DateTimeOffset.UtcNow;
}