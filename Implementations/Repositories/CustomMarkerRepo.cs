using LocationTracker.Context;
using LocationTracker.Entities;
using LocationTracker.Implementations.Repositories;
using LocationTracker.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocationTracker.Implementations.Repositories;
public class CustomMarkerRepo : BaseRepository<CustomMarker>, ICustomMarkerRepo
{
    public CustomMarkerRepo(LocationTrackerContext _context)
    {
        context = _context;
    }
    public async Task<CustomMarker> GetById(int id)
    {
        return await context.CustomMarker.SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }
    public async Task<List<CustomMarker>> GetByPersonId(int personId)
    {
        return await context.CustomMarker.Where(x => x.UserId == personId && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<CustomMarker>> List()
    {
        return await context.CustomMarker.Where(x => x.IsDeleted == false).ToListAsync();
    }
}