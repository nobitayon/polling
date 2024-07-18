using Delta.Polling.Infrastructure.Database.Migrators;
using Delta.Polling.Infrastructure.Database.Seeders;
using Delta.Polling.Services.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Delta.Polling.Infrastructure.Database;

public static class ConfigureDatabase
{
    public static void AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = configuration.GetSection(DatabaseOptions.SectionKey).Get<DatabaseOptions>()
            ?? throw new ConfigurationBindingFailedException(DatabaseOptions.SectionKey, typeof(DatabaseOptions));

        _ = services.AddDbContext<IDatabaseService, DatabaseService>(options =>
        {
            _ = options.UseSqlServer(databaseOptions.ConnectionString);
        });

        _ = services.AddScoped<DatabaseMigrator>();
        _ = services.AddScoped<MovieSeeder>();
    }

    public static async Task InitializeDatabaseAsync(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        var serviceProvider = serviceScope.ServiceProvider;

        var databaseMigrator = serviceProvider.GetRequiredService<DatabaseMigrator>();
        await databaseMigrator.MigrateAsync(serviceProvider);

        var movieSeeder = serviceProvider.GetRequiredService<MovieSeeder>();
        await movieSeeder.SeedMovies();
    }
}
