using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovies;

namespace Delta.Polling.Logics.Contributor.Movies.Queries.GetMyMovies;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record GetMyMoviesQuery : GetMyMoviesRequest, IRequest<GetMyMoviesOutput>
{
}

public class GetMyMoviesQueryValidator : AbstractValidator<GetMyMoviesQuery>
{
    public GetMyMoviesQueryValidator()
    {
        Include(new GetMyMoviesRequestValidator());
    }
}

public class GetMyMoviesQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMyMoviesQuery, GetMyMoviesOutput>
{
    public async Task<GetMyMoviesOutput> Handle(GetMyMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await databaseService.Movies
            .Where(movie => movie.CreatedBy == currentUserService.Username)
            .OrderByDescending(movie => movie.Created)
            .Take(request.MaxCount)
            .Select(movie => new MovieItem
            {
                Id = movie.Id,
                Title = movie.Title,
                Budget = movie.Budget,
                Status = movie.Status
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var output = new GetMyMoviesOutput
        {
            Data = movies
        };

        return output;
    }
}
