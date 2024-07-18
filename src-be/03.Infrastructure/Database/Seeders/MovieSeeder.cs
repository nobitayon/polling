using Delta.Polling.Base.Movies.Enums;
using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Seeders;

public class MovieSeeder(
    ILogger<MovieSeeder> logger,
    IDatabaseService databaseService)
{
    public static readonly List<Movie> InitialMovies =
    [
        new()
        {
            Created = DateTimeOffset.Now,
            CreatedBy = "user.satu",
            Title = "Shawshank Redemption",
            Storyline = "Lorem ipsum",
            Budget = 1_000_000,
            Status = MovieStatus.Draft
        },
        new()
        {
            Created = DateTimeOffset.Now,
            CreatedBy = "user.satu",
            Title = "The Godfather",
            Storyline = "Lorem ipsum",
            Budget = 1_500_000,
            Status = MovieStatus.Draft
        },
        new()
        {
            Created = DateTimeOffset.Now,
            CreatedBy = "user.satu",
            Title = "The Dark Knight",
            Storyline = "Bla bla bla",
            Budget = 1_200_000,
            Status = MovieStatus.Draft
        },
        new()
        {
            Created = DateTimeOffset.Now,
            CreatedBy = "user.satu",
            Title = "The Lord of the Rings: The Return of the King",
            Storyline = "Bla bla bla",
            Budget = 1_300_000,
            Status = MovieStatus.Draft
        },
        new()
        {
            Created = DateTimeOffset.Now,
            CreatedBy = "user.satu",
            Title = "Pulp Fiction",
            Storyline = "Bla bla bla",
            Budget = 1_100_000,
            Status = MovieStatus.Draft
        },
        new()
        {
            Created = DateTimeOffset.Now,
            CreatedBy = "user.satu",
            Title = "Forrest Gump",
            Storyline = "USA",
            Budget = 1_400_000,
            Status = MovieStatus.Draft
        }
    ];

    public async Task SeedMovies()
    {
        logger.LogInformation("Seeding data {EntityType}...", "Movie");

        if (!databaseService.Movies.Any())
        {
            foreach (var initialMovie in InitialMovies)
            {
                _ = await databaseService.Movies.AddAsync(initialMovie);
            }
        }

        _ = await databaseService.SaveAsync();
    }
}
