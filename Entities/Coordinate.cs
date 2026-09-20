using System.ComponentModel.DataAnnotations;
using LocationTracker.Contracts;
namespace LocationTracker.Entities;
public class Coordinate : AuditableEntity
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public int JourneyId { get; set; }
    public int? RouteId { get; set; }
    public long SequenceIndex { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public float? AccuracyMeters { get; set; }
    public float? HeadingDegrees { get; set; }
    public float SpeedKmH { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}
