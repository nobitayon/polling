using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace Delta.Polling.Infrastructure.Database;

public class DatabaseService(DbContextOptions<DatabaseService> options)
    : DbContext(options), IDatabaseService
{
    public DbSet<Movie> Movies => Set<Movie>();

    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseService).Assembly);
    }
}
