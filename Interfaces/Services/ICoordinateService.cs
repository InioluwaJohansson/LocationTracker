using LocationTracker.Entities;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Interfaces.Services;
public interface ICoordinateService
{
    public Task<List<GetCoordinateDto>> GetCoordinatesByJourneyId(int journeyId);
    public Task<BaseResponse> CreateCoordinate(CreateCoordinateDto coordinate);
    public Task<BaseResponse> CreateBatchCoordinates(List<CreateCoordinateDto> coordinates);
    public Task<List<GetCoordinateDto>> GetCoordinatesByRouteId(int routeId);
}