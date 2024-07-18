using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovie;

namespace Delta.Polling.Logics.Contributor.Movies.Queries.GetMyMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record GetMyMovieQuery : GetMyMovieRequest, IRequest<GetMyMovieOutput>
{
}

public class GetMyMovieQueryValidator : AbstractValidator<GetMyMovieQuery>
{
    public GetMyMovieQueryValidator()
    {
        Include(new GetMyMovieRequestValidator());
    }
}

public class GetMyMovieQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMyMovieQuery, GetMyMovieOutput>
{
    public async Task<GetMyMovieOutput> Handle(GetMyMovieQuery request, CancellationToken cancellationToken)
    {
        var movie = await databaseService.Movies
              .SingleOrDefaultAsync(movie => movie.Id == request.MovieId, cancellationToken)
              ?? throw new EntityNotFoundException("Movie", request.MovieId);

        if (movie.CreatedBy != currentUserService.Username)
        {
            throw new Exception($"You cannot get Movie with Id {request.MovieId} because it is not yours.");
        }

        var movieItem = new MovieItem
        {
            Id = movie.Id,
            Title = movie.Title,
            Storyline = movie.Storyline,
            Budget = movie.Budget,
            Created = DateTimeOffset.Now,
            Status = movie.Status,
            Approved = movie.Approved,
            ApprovedBy = movie.ApprovedBy,
        };

        return new GetMyMovieOutput
        {
            Data = movieItem
        };
    }
}
