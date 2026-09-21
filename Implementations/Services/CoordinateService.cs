using Home_Security.RealTimeServices;
using LocationTracker.Entities;
using LocationTracker.Interfaces.Repositories;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Implementations.Services;
public class CoordinateService : ICoordinateService
{
    ICoordinateRepo _coordinateRepo;
    IRealtimeNotificationService _realtime;
    public CoordinateService(ICoordinateRepo coordinateRepo, IRealtimeNotificationService realtime)
    {
        _coordinateRepo = coordinateRepo;
        _realtime = realtime;
    }
    public async Task<BaseResponse> CreateCoordinate(CreateCoordinateDto coordinate)
    {
        var newCoordinate = new Coordinate
        {
            RouteId = coordinate.RouteId,
            UserId = coordinate.UserId,
            Latitude = coordinate.Latitude,
            Longitude = coordinate.Longitude,
            SequenceIndex = coordinate.SequenceIndex,
            Altitude = coordinate.Altitude,
            AccuracyMeters = coordinate.AccuracyMeters,
            HeadingDegrees = coordinate.HeadingDegrees,
            SpeedKmH = coordinate.SpeedKmH,
            Timestamp = DateTimeOffset.UtcNow,
            IsDeleted = false,
            LastModifiedOn = DateTime.UtcNow,
            LastModifiedBy = coordinate.UserId,
            CreatedBy = coordinate.UserId
        };
        newCoordinate = await _coordinateRepo.Create(newCoordinate);
        await _realtime.NotifyAll("New Coordinate Added", newCoordinate);
        return new BaseResponse { Success = true, Message = "Coordinate created successfully." };
    }
    public async Task<BaseResponse> CreateBatchCoordinates(List<CreateCoordinateDto> coordinates)
    {
        foreach (var coordinate in coordinates)
        {
            var newCoordinate = new Coordinate
            {
                RouteId = coordinate.RouteId,
                UserId = coordinate.UserId,
                Latitude = coordinate.Latitude,
                Longitude = coordinate.Longitude,
                SequenceIndex = coordinate.SequenceIndex,
                Altitude = coordinate.Altitude,
                AccuracyMeters = coordinate.AccuracyMeters,
                HeadingDegrees = coordinate.HeadingDegrees,
                SpeedKmH = coordinate.SpeedKmH,
                Timestamp = DateTimeOffset.UtcNow,
                IsDeleted = false,
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = coordinate.UserId,
                CreatedBy = coordinate.UserId
            };
            newCoordinate = await _coordinateRepo.Create(newCoordinate);
        }
        await _realtime.NotifyAll("New Coordinates Added", coordinates);
        return new BaseResponse { Success = true, Message = "Batch of coordinates created successfully." };
    }
    public async Task<List<GetCoordinateDto>> GetCoordinatesByJourneyId(int journeyId)
    {
        var coordinates = await _coordinateRepo.GetByJourneyId(journeyId);
        return coordinates.Select(x => new GetCoordinateDto
        {
            Id = x.Id,
            UserId = x.UserId,
            JourneyId = x.JourneyId,
            RouteId = x.RouteId,
            SequenceIndex = x.SequenceIndex,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
            Altitude = x.Altitude,
            AccuracyMeters = x.AccuracyMeters,
            HeadingDegrees = x.HeadingDegrees,
            SpeedKmH = x.SpeedKmH,
            Timestamp = x.Timestamp
        }).ToList();
    }
    public async Task<List<GetCoordinateDto>> GetCoordinatesByRouteId(int routeId)
    {
        var coordinates = await _coordinateRepo.GetByRouteId(routeId);
        return coordinates.Select(x => new GetCoordinateDto
        {
            Id = x.Id,
            UserId = x.UserId,
            JourneyId = x.JourneyId,
            RouteId = x.RouteId,
            SequenceIndex = x.SequenceIndex,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
            Altitude = x.Altitude,
            AccuracyMeters = x.AccuracyMeters,
            HeadingDegrees = x.HeadingDegrees,
            SpeedKmH = x.SpeedKmH,
            Timestamp = x.Timestamp
        }).ToList();
    }
}