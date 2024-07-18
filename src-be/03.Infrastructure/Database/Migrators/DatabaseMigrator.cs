using Microsoft.EntityFrameworkCore;

namespace Delta.Polling.Infrastructure.Database.Migrators;

public class DatabaseMigrator(ILogger<DatabaseMigrator> logger)
{
    public async Task MigrateAsync(IServiceProvider serviceProvider)
    {
        var databaseService = serviceProvider.GetRequiredService<DatabaseService>();

        var pendingMigrations = await databaseService.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying database migration...");

            await databaseService.Database.MigrateAsync();
        }
        else
        {
            logger.LogInformation("Database is up to date. No database migration required.");
        }
    }
}
