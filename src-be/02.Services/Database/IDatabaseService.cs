using Delta.Polling.Domain.Movies.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delta.Polling.Services.Database;

public interface IDatabaseService
{
    DbSet<Movie> Movies { get; }

    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
