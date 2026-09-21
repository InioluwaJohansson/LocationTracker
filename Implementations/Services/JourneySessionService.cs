using Home_Security.RealTimeServices;
using LocationTracker.Entities;
using LocationTracker.Interfaces.Repositories;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Implementations.Services;
public class JourneySessionService : IJourneySessionService
{
    private readonly ICoordinateRepo _coordinateRepo;
    private readonly IJourneySessionRepo _journeySessionRepo;   
    private readonly IRealtimeNotificationService _realtimeNotificationService; 
    public JourneySessionService(ICoordinateRepo coordinateRepo, IJourneySessionRepo journeySessionRepo, IRealtimeNotificationService realtimeNotificationService)
    {
        _coordinateRepo = coordinateRepo;
        _journeySessionRepo = journeySessionRepo;
        _realtimeNotificationService = realtimeNotificationService;
    }
    public async Task<BaseResponse> CreateJourneySession(CreateJourneySessionDto createJourneySessionDto)
    {
        var newJourneySession = new JourneySession
        {
            UserId = createJourneySessionDto.UserId,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = createJourneySessionDto.EndTime,
            TotalDistanceMeters = createJourneySessionDto.TotalDistanceMeters,
            TotalDurationSeconds = createJourneySessionDto.TotalDurationSeconds,
            AverageSpeedKmH = createJourneySessionDto.AverageSpeedKmH,
            Status = createJourneySessionDto.Status,
            IsDeleted = false,
            LastModifiedOn = DateTime.UtcNow,
            LastModifiedBy = createJourneySessionDto.UserId,
            CreatedBy = createJourneySessionDto.UserId,
            Coordinates = createJourneySessionDto.Coordinates?.Select(c => new Coordinate
            {
                RouteId = c.RouteId,
                UserId = createJourneySessionDto.UserId,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                SequenceIndex = c.SequenceIndex,
                Altitude = c.Altitude,
                AccuracyMeters = c.AccuracyMeters,
                HeadingDegrees = c.HeadingDegrees,
                SpeedKmH = c.SpeedKmH,
                Timestamp = DateTimeOffset.UtcNow,
                IsDeleted = false,
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = createJourneySessionDto.UserId,
                CreatedBy = createJourneySessionDto.UserId
            }).ToList() ?? new List<Coordinate>(),
        };
        await _journeySessionRepo.Create(newJourneySession);
        await _realtimeNotificationService.NotifyAll("Journey Session Started", newJourneySession);
        return new BaseResponse 
        { 
            Success = true, 
            Message = "Journey session created successfully." 
        };
    }
    public async Task<BaseResponse> UpdateJourneySession(UpdateJourneySessionDto updateJourneySessionDto)
    {
        var existingJourneySession = await _journeySessionRepo.Get(x => x.Id == updateJourneySessionDto.Id);
        if (existingJourneySession == null)
        {
            return new BaseResponse { Success = false, Message = "Journey session not found." };
        }

        existingJourneySession.UserId = updateJourneySessionDto.UserId;
        existingJourneySession.StartTime = updateJourneySessionDto.StartTime;
        existingJourneySession.EndTime = updateJourneySessionDto.EndTime;
        existingJourneySession.TotalDistanceMeters = updateJourneySessionDto.TotalDistanceMeters;
        existingJourneySession.TotalDurationSeconds = updateJourneySessionDto.TotalDurationSeconds;
        existingJourneySession.AverageSpeedKmH = updateJourneySessionDto.AverageSpeedKmH;
        existingJourneySession.Status = updateJourneySessionDto.Status;
        existingJourneySession.LastModifiedOn = DateTime.UtcNow;
        existingJourneySession.LastModifiedBy = updateJourneySessionDto.UserId;

        await _journeySessionRepo.Update(existingJourneySession);
        await _realtimeNotificationService.NotifyAll("Journey Session Updated", existingJourneySession);
        return new BaseResponse { Success = true, Message = "Journey session updated successfully." };
    }
    public async Task<List<GetJourneySessionDto>> GetAllJourneySessions()
    {
        var journeySessions = await _journeySessionRepo.List();
        return journeySessions.Select(x => new GetJourneySessionDto
        {
            Id = x.Id,
            UserId = x.UserId,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            TotalDistanceMeters = x.TotalDistanceMeters,
            TotalDurationSeconds = x.TotalDurationSeconds,
            AverageSpeedKmH = x.AverageSpeedKmH,
            Status = x.Status,
            Coordinates = x.Coordinates?.Select(c => new GetCoordinateDto
            {
                Id = c.Id,
                RouteId = c.RouteId,
                UserId = c.UserId,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                SequenceIndex = c.SequenceIndex,
                Altitude = c.Altitude,
                AccuracyMeters = c.AccuracyMeters,
                HeadingDegrees = c.HeadingDegrees,
                SpeedKmH = c.SpeedKmH,
                Timestamp = c.Timestamp
            }).ToList() ?? new List<GetCoordinateDto>()
        }).ToList();
    }
    public async Task<BaseResponse> CancelJourneySession(int journeySessionId)
    {
        var existingJourneySession = await _journeySessionRepo.Get(x => x.Id == journeySessionId);
        if (existingJourneySession == null)
        {
            return new BaseResponse { Success = false, Message = "Journey session not found." };
        }
        existingJourneySession.Status = Models.Enums.JourneyStatus.Cancelled;
        existingJourneySession.LastModifiedOn = DateTime.UtcNow;
        existingJourneySession.LastModifiedBy = existingJourneySession.UserId;
        await _journeySessionRepo.Update(existingJourneySession);
        await _realtimeNotificationService.NotifyAll("Journey Session Cancelled", existingJourneySession);
        return new BaseResponse { Success = true, Message = "Journey session cancelled successfully." };
    }
}