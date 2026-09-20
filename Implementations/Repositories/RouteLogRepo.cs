using LocationTracker.Context;
using LocationTracker.Entities;
using LocationTracker.Implementations.Repositories;
using LocationTracker.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocationTracker.Implementations.Repositories;
public class RouteLogRepo : BaseRepository<CompletedRouteLog>, IRouteLogRepo
{
    public RouteLogRepo(LocationTrackerContext _context)
    {
        context = _context;
    }
    public async Task<CompletedRouteLog> GetById(int id)
    {
        return await context.CompletedRouteLog.SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }
    public async Task<List<CompletedRouteLog>> GetByPersonId(int personId)
    {
        return await context.CompletedRouteLog.Where(x => x.UserId == personId && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<CompletedRouteLog>> List()
    {
        return await context.CompletedRouteLog.Where(x => x.IsDeleted == false).ToListAsync();
    }
}