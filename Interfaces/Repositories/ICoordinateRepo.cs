using LocationTracker.Entities;

namespace LocationTracker.Interfaces.Repositories;
public interface ICoordinateRepo : IRepo<Coordinate>
{
    public Task<Coordinate> GetById(int id);
    public Task<List<Coordinate>> GetByPersonId(int personId);
    public Task<List<Coordinate>> GetByJourneyId(int journeyId);
    public Task<List<Coordinate>> GetByRouteId(int routeId);
    public Task<List<Coordinate>> List();
}
