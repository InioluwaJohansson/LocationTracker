using System.ComponentModel.DataAnnotations;

namespace LocationTracker.Models.DTOs;
public class CreateCoordinateDto
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
}
public class GetCoordinateDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int JourneyId { get; set; }
    public int? RouteId { get; set; }
    public long SequenceIndex { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public float? AccuracyMeters { get; set; }
    public float? HeadingDegrees { get; set; }
    public float SpeedKmH { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}