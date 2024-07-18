using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Domain.Polls.Entities;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Seeders;

public class PollSeeder(
    ILogger<PollSeeder> logger,
    IDatabaseService databaseService)
{

    public async Task SeedPolls()
    {
        logger.LogInformation("Seeding data {EntityType}...", "Poll");

        if (!databaseService.Polls.Any())
        {

            var groupBola = databaseService.Groups.Single(group => group.Name == "Group Bola");
            var groupVolly = databaseService.Groups.Single(group => group.Name == "Group Volly");
            var ktbffh = databaseService.Groups.Single(group => group.Name == "KTBFFH");
            var epl = databaseService.Groups.Single(group => group.Name == "EPL");
            var serieA = databaseService.Groups.Single(group => group.Name == "Serie A");

            List<Poll> initialPolls =
            [
                new Poll {Created = DateTimeOffset.Now, CreatedBy = "polling.member.satu", GroupId = groupBola.Id, MaximumAnswer = 1, AllowOtherChoice = true, Title = "GOAT ?", Question = "GOAT between these two", Status = PollStatus.Draft},
                new Poll {Created = DateTimeOffset.Now, CreatedBy = "polling.member.satu", GroupId = groupVolly.Id, MaximumAnswer = 5, AllowOtherChoice = true, Title = "recommended volly ball ?", Question = "what is your recommendation of volly ball", Status = PollStatus.Draft},
                new Poll {Created = DateTimeOffset.Now, CreatedBy = "polling.member.dua", GroupId = ktbffh.Id, MaximumAnswer = 1, AllowOtherChoice = false, Title = "Best scorer at chelsea of all time ?", Question = "Pick one between these 3", Status = PollStatus.Ongoing},
                new Poll {Created = DateTimeOffset.Now, CreatedBy = "polling.member.satu", GroupId = epl.Id, MaximumAnswer = 2, AllowOtherChoice = true, Title = "cool epl club name ?", Question = "cool epl club name", Status = PollStatus.Draft},
                new Poll {Created = DateTimeOffset.Now, CreatedBy = "polling.member.empat", GroupId = serieA.Id, MaximumAnswer = 3, AllowOtherChoice = true, Title = "list of footballer street never forget", Question = "pick the legend", Status = PollStatus.Ongoing}
            ];

            await databaseService.Polls.AddRangeAsync(initialPolls);

            _ = await databaseService.SaveAsync();
        }
    }
}
