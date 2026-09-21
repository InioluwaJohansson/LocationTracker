using Home_Security.RealTimeServices;
using LocationTracker.Entities;
using LocationTracker.Interfaces.Repositories;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Implementations.Services;
public class RouteLogService : IRouteLogService
{
    private readonly IRouteLogRepo _routeLogRepo;
    private readonly IRealtimeNotificationService _realTimeNotificationService;
    public RouteLogService(IRouteLogRepo routeLogRepo, IRealtimeNotificationService realTimeNotificationService)
    {
        _routeLogRepo = routeLogRepo;
        _realTimeNotificationService = realTimeNotificationService;
    }
    public async Task<BaseResponse> CreateRouteLog(CreateRouteLogDto routeLog)
    {
        var newRouteLog = new CompletedRouteLog
        {
            UserId = routeLog.UserId,
            JourneyId = routeLog.JourneyId,
            RouteId = routeLog.RouteId,
            OriginName = routeLog.OriginName,
            DestinationName = routeLog.DestinationName,
            TotalDistanceMeters = routeLog.TotalDistanceMeters,
            DurationSeconds = routeLog.DurationSeconds,
            TravelMode = routeLog.TravelMode,
            CompletedAt = DateTimeOffset.UtcNow,
            IsDeleted = false,
            LastModifiedOn = DateTime.UtcNow,
            LastModifiedBy = routeLog.UserId,
            CreatedBy = routeLog.UserId,
        };
        await _routeLogRepo.Create(newRouteLog);
        await _realTimeNotificationService.NotifyAll("Route Log Added", newRouteLog);
        return new BaseResponse 
        { 
            Success = true, 
            Message = "Route log created successfully." 
        };
    }
    public async Task<BaseResponse> UpdateRouteLog(UpdateRouteLogDto routeLog)
    {
        var existingRouteLog = await _routeLogRepo.Get(x => x.Id == routeLog.Id);
        if (existingRouteLog == null)
        {
            return new BaseResponse { Success = false, Message = "Route log not found." };
        }

        existingRouteLog.OriginName = routeLog.OriginName;
        existingRouteLog.DestinationName = routeLog.DestinationName;
        existingRouteLog.TotalDistanceMeters = routeLog.TotalDistanceMeters;
        existingRouteLog.DurationSeconds = routeLog.DurationSeconds;
        existingRouteLog.TravelMode = routeLog.TravelMode;
        existingRouteLog.LastModifiedOn = DateTime.UtcNow;
        existingRouteLog.LastModifiedBy = routeLog.UserId;

        await _routeLogRepo.Update(existingRouteLog);
        await _realTimeNotificationService.NotifyAll("Route Log Updated", existingRouteLog);
        return new BaseResponse { Success = true, Message = "Route log updated successfully." };
    }
    public async Task<List<GetRouteLogDto>> GetAllRouteLogs()
    {
        var routeLogs = await _routeLogRepo.List();
        return routeLogs.Select(x => new GetRouteLogDto
        {
            Id = x.Id,
            UserId = x.UserId,
            JourneyId = x.JourneyId,
            RouteId = x.RouteId,
            OriginName = x.OriginName,
            DestinationName = x.DestinationName,
            TotalDistanceMeters = x.TotalDistanceMeters,
            DurationSeconds = x.DurationSeconds,
            TravelMode = x.TravelMode,
            CompletedAt = x.CompletedAt
        }).ToList();
    }
    public async Task<BaseResponse> DeleteRouteLog(int routeLogId)
    {
        var existingRouteLog = await _routeLogRepo.Get(x => x.Id == routeLogId);
        if (existingRouteLog == null)
        {
            return new BaseResponse { Success = false, Message = "Route log not found." };
        }
        existingRouteLog.IsDeleted = true;
        existingRouteLog.LastModifiedOn = DateTime.UtcNow;
        await _routeLogRepo.Update(existingRouteLog);
        return new BaseResponse { Success = true, Message = "Route log deleted successfully." };
    }
}