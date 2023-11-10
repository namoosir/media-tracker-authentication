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

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(
                    e =>
                        e.Entity is PlatformConnection
                        && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entityEntry in entries)
            {
                ((PlatformConnection)entityEntry.Entity).UpdatedAt = DateTime.Now;
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(
                    e =>
                        e.Entity is PlatformConnection
                        && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entityEntry in entries)
            {
                ((PlatformConnection)entityEntry.Entity).UpdatedAt = DateTime.Now;
            }
            
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(
                    e =>
                        e.Entity is PlatformConnection
                        && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entityEntry in entries)
            {
                ((PlatformConnection)entityEntry.Entity).UpdatedAt = DateTime.Now;
            }
            
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
