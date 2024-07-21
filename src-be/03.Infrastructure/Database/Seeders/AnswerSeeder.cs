using Delta.Polling.Domain.Answers.Entities;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Seeders;

public class AnswerSeeder(
    ILogger<AnswerSeeder> logger,
    IDatabaseService databaseService)
{

    public async Task SeedAnswers()
    {
        logger.LogInformation("Seeding data {EntityType}...", "Answer");

        if (!databaseService.Answers.Any())
        {
            var ongoingPoll = databaseService.Polls.Single(poll => poll.Title == "list of footballer street never forget");

            var choice1 = databaseService.Choices.Single(choice => choice.Description == "Taarabt");
            var choice2 = databaseService.Choices.Single(choice => choice.Description == "Obi");

            var member1 = "polling.member.satu";
            var member4 = "polling.member.empat";

            var voterMember1 = databaseService.Voters.Single(voter => voter.Username == member1);
            var voterMember4 = databaseService.Voters.Single(voter => voter.Username == member4);

            List<Answer> initialAnswers =
            [
                new Answer {ChoiceId = choice1.Id, VoterId = voterMember1.Id, Created = DateTimeOffset.Now, CreatedBy = member1},
                new Answer {ChoiceId = choice2.Id, VoterId = voterMember1.Id, Created = DateTimeOffset.Now, CreatedBy = member1},
                new Answer {ChoiceId = choice2.Id, VoterId = voterMember4.Id, Created = DateTimeOffset.Now, CreatedBy = member4},
            ];

            foreach (var initialAnswer in initialAnswers)
            {
                _ = await databaseService.Answers.AddAsync(initialAnswer);
            }
        }

        _ = await databaseService.SaveAsync();
    }
}
