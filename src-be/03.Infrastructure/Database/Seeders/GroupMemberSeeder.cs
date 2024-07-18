using Delta.Polling.Domain.Groups.Entities;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Seeders;

public class GroupMemberSeeder(
    ILogger<GroupMemberSeeder> logger,
    IDatabaseService databaseService)
{

    public async Task SeedGroupMembers()
    {
        logger.LogInformation("Seeding data {EntityType}...", "Group Member");

        if (!databaseService.GroupMembers.Any())
        {

            var groupBola = databaseService.Groups.Single(group => group.Name == "Group Bola");
            var groupVolly = databaseService.Groups.Single(group => group.Name == "Group Volly");
            var ktbffh = databaseService.Groups.Single(group => group.Name == "KTBFFH");
            var epl = databaseService.Groups.Single(group => group.Name == "EPL");
            var serieA = databaseService.Groups.Single(group => group.Name == "Serie A");

            List<GroupMember> initialGroupMembers =
            [
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.satu", GroupId = groupBola.Id, Username = "polling.member.satu"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.satu", GroupId = groupBola.Id, Username = "polling.member.dua"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.satu", GroupId = groupBola.Id, Username = "polling.member.tiga"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.satu", GroupId = groupBola.Id, Username = "polling.member.empat"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.dua", GroupId = groupVolly.Id, Username = "polling.member.dua"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.dua", GroupId = groupVolly.Id, Username = "polling.member.tiga"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.dua", GroupId = groupVolly.Id, Username = "polling.member.satu"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.satu", GroupId = ktbffh.Id, Username = "polling.member.dua"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.satu", GroupId = ktbffh.Id, Username = "polling.member.tiga"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.satu", GroupId = ktbffh.Id, Username = "polling.member.empat"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.dua", GroupId = serieA.Id, Username = "polling.member.satu"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.dua", GroupId = serieA.Id, Username = "polling.member.empat"},
                new GroupMember {Created=DateTimeOffset.Now, CreatedBy = "polling.admin.satu", GroupId = epl.Id, Username = "polling.member.satu"},
            ];

            await databaseService.GroupMembers.AddRangeAsync(initialGroupMembers);

            _ = await databaseService.SaveAsync();
        }
    }
}
