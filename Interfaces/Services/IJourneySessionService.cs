using LocationTracker.Entities;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Interfaces.Services;
public interface IJourneySessionService
{
    public Task<BaseResponse> CreateJourneySession(CreateJourneySessionDto journeySession);
    public Task<BaseResponse> UpdateJourneySession(UpdateJourneySessionDto journeySession);
    public Task<GetJourneySessionDto> GetJourneySessionById(int id);
    public Task<List<GetJourneySessionDto>> GetAllJourneySessions(DateTime date);
    public Task<BaseResponse> CancelJourneySession(int journeySessionId);
}