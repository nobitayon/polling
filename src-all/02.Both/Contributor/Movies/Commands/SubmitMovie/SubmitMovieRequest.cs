namespace Delta.Polling.Both.Contributor.Movies.Commands.SubmitMovie;

public abstract record SubmitMovieRequest
{
    public required Guid MovieId { get; init; }
}

public class SubmitMovieRequestValidator : AbstractValidator<SubmitMovieRequest>
{
    public SubmitMovieRequestValidator()
    {
        _ = RuleFor(input => input.MovieId)
            .NotEmpty();
    }
}
