using LocationTracker.Context;
using LocationTracker.Entities;
using LocationTracker.Implementations.Repositories;
using LocationTracker.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocationTracker.Implementations.Repositories;
public class CoordinateRepo : BaseRepository<Coordinate>, ICoordinateRepo
{
    public CoordinateRepo(LocationTrackerContext _context)
    {
        context = _context;
    }
    public async Task<Coordinate> GetById(int id)
    {
        return await context.Coordinate.SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }
    public async Task<List<Coordinate>> GetByPersonId(int personId)
    {
        return await context.Coordinate.Where(x => x.UserId == personId && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<Coordinate>> GetByJourneyId(int journeyId)
    {
        return await context.Coordinate.Where(x => x.JourneyId == journeyId && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<Coordinate>> GetByRouteId(int routeId)
    {
        return await context.Coordinate.Where(x => x.RouteId == routeId && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<Coordinate>> List()
    {
        return await context.Coordinate.Where(x => x.IsDeleted == false).ToListAsync();
    }
}