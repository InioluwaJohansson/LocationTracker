using LocationTracker.Entities;

namespace LocationTracker.Interfaces.Repositories;
public interface ICustomMarkerRepo : IRepo<CustomMarker>
{
    public Task<CustomMarker> GetById(int id);
    public Task<List<CustomMarker>> GetByPersonId(int personId);
    public Task<List<CustomMarker>> List();
}