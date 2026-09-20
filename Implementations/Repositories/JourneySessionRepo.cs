using LocationTracker.Context;
using LocationTracker.Entities;
using LocationTracker.Implementations.Repositories;
using LocationTracker.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocationTracker.Implementations.Repositories;
public class JourneySessionRepo : BaseRepository<JourneySession>, IJourneySessionRepo
{
    public JourneySessionRepo(LocationTrackerContext _context)
    {
        context = _context;
    }
    public async Task<JourneySession> GetById(int id)
    {
        return await context.JourneySession.Include(x => x.Coordinates).SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }
    public async Task<List<JourneySession>> GetByPersonId(int personId)
    {
        return await context.JourneySession.Include(x => x.Coordinates).Where(x => x.UserId == personId && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<JourneySession>> List()
    {
        return await context.JourneySession.Include(x => x.Coordinates).Where(x => x.IsDeleted == false).ToListAsync();
    }
}