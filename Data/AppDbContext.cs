using MediaTrackerAuthenticationService.Models.AuthDB;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerAuthenticationService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<PlatformConnection> PlatformConnections => Set<PlatformConnection>();
        public DbSet<User> Users => Set<User>();
    }
}
