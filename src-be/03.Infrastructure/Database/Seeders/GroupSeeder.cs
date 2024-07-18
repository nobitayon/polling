using Delta.Polling.Domain.Groups.Entities;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Seeders;

public class GroupSeeder(
    ILogger<GroupSeeder> logger,
    IDatabaseService databaseService)
{
    private const string System = "system";

    public static readonly List<Group> InitialGroups =
    [
        new Group {Name = "Group Bola", Created = DateTimeOffset.Now, CreatedBy = "polling.admin.satu"},
        new Group {Name = "Group Volly", Created = DateTimeOffset.Now, CreatedBy = "polling.admin.dua"},
        new Group {Name = "KTBFFH", Created = DateTimeOffset.Now, CreatedBy = "polling.admin.satu"},
        new Group {Name = "EPL", Created = DateTimeOffset.Now, CreatedBy = "polling.admin.dua"},
        new Group {Name = "Serie A", Created = DateTimeOffset.Now, CreatedBy = "polling.admin.satu"},
    ];

    public async Task SeedGroups()
    {
        logger.LogInformation("Seeding data {EntityType}...", "Group");

        if (!databaseService.Groups.Any())
        {
            foreach (var initialGroup in InitialGroups)
            {
                _ = await databaseService.Groups.AddAsync(initialGroup);
            }
        }

        _ = await databaseService.SaveAsync();
    }
}
