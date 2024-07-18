using Delta.Polling.Domain.Answers.Entities;
using Delta.Polling.Domain.Choices.Entities;
using Delta.Polling.Domain.Groups.Entities;
using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Domain.Polls.Entities;
using Delta.Polling.Domain.Voters.Entities;
using Delta.Polling.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace Delta.Polling.Infrastructure.Database;

public class DatabaseService(DbContextOptions<DatabaseService> options)
    : DbContext(options), IDatabaseService
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
    public DbSet<Poll> Polls => Set<Poll>();
    public DbSet<Choice> Choices => Set<Choice>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Voter> Voters => Set<Voter>();

    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseService).Assembly);
    }
}
