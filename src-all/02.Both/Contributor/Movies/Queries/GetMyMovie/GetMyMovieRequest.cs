namespace Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovie;

public record GetMyMovieRequest
{
    public required Guid MovieId { get; set; }
}

public class GetMyMovieRequestValidator : AbstractValidator<GetMyMovieRequest>
{
    public GetMyMovieRequestValidator()
    {
        _ = RuleFor(x => x.MovieId)
            .NotEmpty();
    }
}
