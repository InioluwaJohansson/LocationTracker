using LocationTracker.Entities;

namespace LocationTracker.Interfaces.Repositories;
public interface IRouteLogRepo : IRepo<CompletedRouteLog>
{
    public Task<CompletedRouteLog> GetById(int id);
    public Task<List<CompletedRouteLog>> GetByPersonId(int personId);
    public Task<List<CompletedRouteLog>> List();
}