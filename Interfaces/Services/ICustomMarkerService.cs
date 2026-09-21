using LocationTracker.Entities;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Interfaces.Services;
public interface ICustomMarkerService
{
    public Task<BaseResponse> CreateCustomMarker(CreateCustomMarkerDto customMarker);
    public Task<BaseResponse> UpdateCustomMarker(UpdateCustomMarkerDto customMarker);
    public Task<List<GetCustomMarkerDto>> GetAllCustomMarkers();
    public Task<BaseResponse> DeleteCustomMarker(int customMarkerId);
}