using LocationTracker.Entities;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Interfaces.Services;
public interface IRouteLogService
{
    public Task<BaseResponse> CreateRouteLog(CreateRouteLogDto routeLog);
    public Task<BaseResponse> UpdateRouteLog(UpdateRouteLogDto routeLog);
    public Task<List<GetRouteLogDto>> GetAllRouteLogs();
    public Task<BaseResponse> DeleteRouteLog(int routeLogId);
}