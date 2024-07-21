using Delta.Polling.Domain.Choices.Entities;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Seeders;

public class ChoiceSeeder(
    ILogger<ChoiceSeeder> logger,
    IDatabaseService databaseService)
{

    public async Task SeedChoices()
    {
        logger.LogInformation("Seeding data {EntityType}...", "Choice");

        if (!databaseService.Choices.Any())
        {
            var poll1 = databaseService.Polls.Single(poll => poll.Title == "GOAT ?");
            var poll2 = databaseService.Polls.Single(poll => poll.Title == "list of footballer street never forget");

            List<Choice> initialChoices =
            [
                new Choice {PollId = poll1.Id, Description = "Ronaldo", IsOther = false, Created = DateTimeOffset.Now, CreatedBy = poll1.CreatedBy},
            new Choice {PollId = poll1.Id, Description = "Messi", IsOther = false, Created = DateTimeOffset.Now, CreatedBy = poll1.CreatedBy},
            new Choice {PollId = poll2.Id, Description = "Taarabt", IsOther = false, Created = DateTimeOffset.Now, CreatedBy = "polling.member.dua"},
            new Choice {PollId = poll2.Id, Description = "Obi", IsOther = false, Created = DateTimeOffset.Now, CreatedBy = poll2.CreatedBy},
        ];

            foreach (var initialChoice in initialChoices)
            {
                _ = await databaseService.Choices.AddAsync(initialChoice);
            }
        }

        _ = await databaseService.SaveAsync();
    }
}
