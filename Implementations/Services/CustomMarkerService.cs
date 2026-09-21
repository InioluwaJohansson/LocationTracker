using Home_Security.RealTimeServices;
using LocationTracker.Entities;
using LocationTracker.Interfaces.Repositories;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Implementations.Services;
public class CustomMarkerService : ICustomMarkerService
{
    private readonly ICustomMarkerRepo _customMarkerRepo;
    private readonly IRealtimeNotificationService _realTimeNotificationService;

    public CustomMarkerService(ICustomMarkerRepo customMarkerRepo, IRealtimeNotificationService realTimeNotificationService)
    {
        _customMarkerRepo = customMarkerRepo;
        _realTimeNotificationService = realTimeNotificationService;
    }

    public async Task<BaseResponse> CreateCustomMarker(CreateCustomMarkerDto customMarker)
    {
        var newCustomMarker = new CustomMarker
        {
            UserId = customMarker.UserId,
            Name = customMarker.Name,
            Category = customMarker.Category,
            UserColor = customMarker.UserColor,
            Latitude = customMarker.Latitude,
            Longitude = customMarker.Longitude,
            IsFromStayPoint = customMarker.IsFromStayPoint,
            CreatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false,
            LastModifiedOn = DateTime.UtcNow,
            LastModifiedBy = customMarker.UserId,
            CreatedBy = customMarker.UserId,
        };
        await _customMarkerRepo.Create(newCustomMarker);
        await _realTimeNotificationService.NotifyAll("Marker Added", newCustomMarker);
        await _realTimeNotificationService.NotifyAll("Marker Added", newCustomMarker);

        return new BaseResponse
        {
             Success = true, 
             Message = "Custom marker created successfully." 
        };
    }

    public async Task<BaseResponse> UpdateCustomMarker(UpdateCustomMarkerDto customMarker)
    {
        var existingCustomMarker = await _customMarkerRepo.Get(x => x.Id == customMarker.Id);
        if (existingCustomMarker == null)
        {
            return new BaseResponse { Success = false, Message = "Custom marker not found." };
        }

        existingCustomMarker.Name = customMarker.Name;
        existingCustomMarker.UserColor = customMarker.UserColor;
        existingCustomMarker.Category = customMarker.Category;
        existingCustomMarker.Latitude = customMarker.Latitude;
        existingCustomMarker.Longitude = customMarker.Longitude;
        existingCustomMarker.LastModifiedOn = DateTime.UtcNow;
        existingCustomMarker.LastModifiedBy = customMarker.UserId;

        await _customMarkerRepo.Update(existingCustomMarker);
        await _realTimeNotificationService.NotifyAll("Marker Updated", existingCustomMarker);
        return new BaseResponse { Success = true, Message = "Custom marker updated successfully." };
    }

    public async Task<List<GetCustomMarkerDto>> GetAllCustomMarkers()
    {
        var customMarkers = await _customMarkerRepo.List();
        if (customMarkers == null || !customMarkers.Any())
        {
            return new List<GetCustomMarkerDto>();
        }
        return customMarkers.Select(cm => new GetCustomMarkerDto
        {
            Id = cm.Id,
            UserId = cm.UserId,
            Name = cm.Name,
            Category = cm.Category,
            UserColor = cm.UserColor,
            Latitude = cm.Latitude,
            Longitude = cm.Longitude,
            IsFromStayPoint = cm.IsFromStayPoint,
            CreatedAt = cm.CreatedAt,
        }).ToList();
    }

    public async Task<BaseResponse> DeleteCustomMarker(int customMarkerId)
    {
        var existingCustomMarker = await _customMarkerRepo.Get(x => x.Id == customMarkerId);
        if (existingCustomMarker == null)
        {
            return new BaseResponse { Success = false, Message = "Custom marker not found." };
        }
        existingCustomMarker.IsDeleted = true;
        existingCustomMarker.LastModifiedOn = DateTime.UtcNow;
        await _customMarkerRepo.Delete(existingCustomMarker);
        return new BaseResponse { Success = true, Message = "Custom marker deleted successfully." };
    }
}