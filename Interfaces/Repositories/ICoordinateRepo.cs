using LocationTracker.Entities;

namespace LocationTracker.Interfaces.Repositories;
public interface ICoordinateRepo : IRepo<Coordinate>
{
    public Task<Coordinate> GetById(int id);
    public Task<List<Coordinate>> GetByPersonId(int personId);
    public Task<List<Coordinate>> List();
}
