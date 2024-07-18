using Delta.Polling.Domain.Voters.Entities;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Seeders;

public class VoterSeeder(
    ILogger<VoterSeeder> logger,
    IDatabaseService databaseService)
{

    public async Task SeedVoters()
    {
        logger.LogInformation("Seeding data {EntityType}...", "Voter");

        var ongoingPoll = databaseService.Polls.Single(poll => poll.Title == "list of footballer street never forget");

        var member1 = "polling.member.satu";
        var member4 = "polling.member.empat";

        List<Voter> initialVoters =
        [
            new Voter {PollId = ongoingPoll.Id, Username = member1, Created = DateTimeOffset.Now, CreatedBy = member1},
            new Voter {PollId = ongoingPoll.Id, Username = member4, Created = DateTimeOffset.Now, CreatedBy = member4},
        ];

        if (!databaseService.Voters.Any())
        {
            foreach (var initialVoter in initialVoters)
            {
                _ = await databaseService.Voters.AddAsync(initialVoter);
            }
        }

        _ = await databaseService.SaveAsync();
    }
}
