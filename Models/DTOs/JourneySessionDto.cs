using LocationTracker.Entities;
using LocationTracker.Models.Enums;

namespace LocationTracker.Models.DTOs;
public class CreateJourneySessionDto
{
    public int UserId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public float TotalDistanceMeters { get; set; }
    public int TotalDurationSeconds { get; set; }
    public float AverageSpeedKmH { get; set; }
    public JourneyStatus Status { get; set; }
    public List<CreateCoordinateDto>? Coordinates { get; set; } = new();
}
public class GetJourneySessionDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public float TotalDistanceMeters { get; set; }
    public int TotalDurationSeconds { get; set; }
    public float AverageSpeedKmH { get; set; }
    public JourneyStatus Status { get; set; }
    public List<GetCoordinateDto>? Coordinates { get; set; } = new();
}
public class UpdateJourneySessionDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public float TotalDistanceMeters { get; set; }
    public int TotalDurationSeconds { get; set; }
    public float AverageSpeedKmH { get; set; }
    public JourneyStatus Status { get; set; }
}