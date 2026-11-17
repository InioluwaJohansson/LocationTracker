using LocationTracker.Context;
using LocationTracker.Entities;
using LocationTracker.Implementations.Repositories;
using LocationTracker.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocationTracker.Implementations.Repositories;
public class ActionRepo : BaseRepository<Actions>, IActionRepo
{
    public ActionRepo(LocationTrackerContext _context)
    {
        context = _context;
    }
}