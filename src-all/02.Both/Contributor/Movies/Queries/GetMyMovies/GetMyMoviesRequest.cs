namespace Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovies;

public record GetMyMoviesRequest
{
    public required int MaxCount { get; set; }
}

public class GetMyMoviesRequestValidator : AbstractValidator<GetMyMoviesRequest>
{
    public GetMyMoviesRequestValidator()
    {
        _ = RuleFor(x => x.MaxCount)
            .NotEmpty();
    }
}
