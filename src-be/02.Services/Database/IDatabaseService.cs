using Delta.Polling.Domain.Answers.Entities;
using Delta.Polling.Domain.Choices.Entities;
using Delta.Polling.Domain.Groups.Entities;
using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Domain.Polls.Entities;
using Delta.Polling.Domain.Voters.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delta.Polling.Services.Database;

public interface IDatabaseService
{
    DbSet<Movie> Movies { get; }
    DbSet<MoviePoster> MoviePosters { get; }
    DbSet<ChoiceMedia> ChoiceMedias { get; }
    DbSet<Answer> Answers { get; }
    DbSet<Choice> Choices { get; }
    DbSet<Group> Groups { get; }
    DbSet<GroupMember> GroupMembers { get; }
    DbSet<Poll> Polls { get; }
    DbSet<Voter> Voters { get; }

    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
