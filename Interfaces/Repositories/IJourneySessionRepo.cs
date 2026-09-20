using LocationTracker.Entities;

namespace LocationTracker.Interfaces.Repositories;
public interface IJourneySessionRepo : IRepo<JourneySession>
{
    public Task<JourneySession> GetById(int id);
    public Task<List<JourneySession>> GetByPersonId(int personId);
    public Task<List<JourneySession>> List();
}