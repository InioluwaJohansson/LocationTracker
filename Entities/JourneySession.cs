using LocationTracker.Contracts;
using LocationTracker.Entities;
namespace LocationTracker.Entities;
public class JourneySession : AuditableEntity
{
    public int UserId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public float TotalDistanceMeters { get; set; }
    public int TotalDurationSeconds { get; set; }
    public float AverageSpeedKmH { get; set; }
    public string Status { get; set; } = "Active"; // "Active", "Completed", "Cancelled"
    public List<Coordinate> Coordinates { get; set; } = new();
}