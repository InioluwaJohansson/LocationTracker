using LocationTracker.Entities;
using Microsoft.EntityFrameworkCore;
namespace LocationTracker.Context;
public class LocationTrackerContext: DbContext
{
    public LocationTrackerContext(DbContextOptions<LocationTrackerContext> optionsBuilder): base(optionsBuilder)
    {
    }
    public DbSet<Coordinate> Coordinate { get; set; }
    public DbSet<CustomMarker> CustomMarker { get; set; }
    public DbSet<JourneySession> JourneySession { get; set; }
    public DbSet<CompletedRouteLog> CompletedRouteLog { get; set; }

}
