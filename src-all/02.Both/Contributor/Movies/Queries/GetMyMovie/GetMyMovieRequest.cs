namespace Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovie;

public abstract record GetMyMovieRequest
{
    public required Guid MovieId { get; set; }
}

public class GetMyMovieRequestValidator : AbstractValidator<GetMyMovieRequest>
{
    public GetMyMovieRequestValidator()
    {
        _ = RuleFor(input => input.MovieId)
            .NotEmpty();
    }
}
