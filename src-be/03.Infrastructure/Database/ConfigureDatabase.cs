using Delta.Polling.Infrastructure.Database.Migrators;
using Delta.Polling.Infrastructure.Database.Seeders;
using Delta.Polling.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace Delta.Polling.Infrastructure.Database;

public static class ConfigureDatabase
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = configuration.GetSection(DatabaseOptions.SectionKey).Get<DatabaseOptions>()
            ?? throw new ConfigurationBindingFailedException(DatabaseOptions.SectionKey, typeof(DatabaseOptions));

        _ = services.AddDbContext<IDatabaseService, DatabaseService>(options =>
        {
            _ = options.UseSqlServer(databaseOptions.ConnectionString, builder =>
            {
                _ = builder.MigrationsAssembly(typeof(DatabaseService).Assembly.FullName);
                _ = builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });

        _ = services.AddScoped<DatabaseMigrator>();
        _ = services.AddScoped<MovieSeeder>();
        _ = services.AddScoped<AnswerSeeder>();
        _ = services.AddScoped<ChoiceSeeder>();
        _ = services.AddScoped<GroupMemberSeeder>();
        _ = services.AddScoped<GroupSeeder>();
        _ = services.AddScoped<PollSeeder>();
        _ = services.AddScoped<VoterSeeder>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        var serviceProvider = serviceScope.ServiceProvider;

        var databaseMigrator = serviceProvider.GetRequiredService<DatabaseMigrator>();
        await databaseMigrator.MigrateAsync(serviceProvider);

        var movieSeeder = serviceProvider.GetRequiredService<MovieSeeder>();
        await movieSeeder.SeedMovies();

        var groupSeeder = serviceProvider.GetRequiredService<GroupSeeder>();
        await groupSeeder.SeedGroups();

        var groupMemberSeeder = serviceProvider.GetRequiredService<GroupMemberSeeder>();
        await groupMemberSeeder.SeedGroupMembers();

        var pollSeeder = serviceProvider.GetRequiredService<PollSeeder>();
        await pollSeeder.SeedPolls();

        var choiceSeeder = serviceProvider.GetRequiredService<ChoiceSeeder>();
        await choiceSeeder.SeedChoices();

        var voterSeeder = serviceProvider.GetRequiredService<VoterSeeder>();
        await voterSeeder.SeedVoters();

        var answerSeeder = serviceProvider.GetRequiredService<AnswerSeeder>();
        await answerSeeder.SeedAnswers();
    }
}
