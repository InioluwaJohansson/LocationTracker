using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LocationTracker.Context;
public class LocationTrackerContextFactory : IDesignTimeDbContextFactory<LocationTrackerContext>
{
    public LocationTrackerContext CreateDbContext(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json").Build();
        var connectionString = config.GetConnectionString("LocationTrackerContext");
        var optionsBuilder = new DbContextOptionsBuilder<LocationTrackerContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        return new LocationTrackerContext(optionsBuilder.Options);
    }
}