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
    public DbSet<Movie> Movies { get; }
    public DbSet<MoviePoster> MoviePosters { get; }
    public DbSet<Answer> Answers { get; }
    public DbSet<Choice> Choices { get; }
    public DbSet<Group> Groups { get; }
    public DbSet<GroupMember> GroupMembers { get; }
    public DbSet<Poll> Polls { get; }
    public DbSet<Voter> Voters { get; }

    public Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
