namespace Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovies;

public record GetMyMoviesRequest : PaginatedListRequest
{
}

public class GetMyMoviesRequestValidator : AbstractValidator<GetMyMoviesRequest>
{
    public GetMyMoviesRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
