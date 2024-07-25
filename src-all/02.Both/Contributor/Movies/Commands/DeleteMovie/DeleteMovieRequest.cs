namespace Delta.Polling.Both.Contributor.Movies.Commands.DeleteMovie;

public abstract record DeleteMovieRequest
{
    public required Guid MovieId { get; init; }
}

public class DeleteMovieRequestValidator : AbstractValidator<DeleteMovieRequest>
{
    public DeleteMovieRequestValidator()
    {
        _ = RuleFor(input => input.MovieId)
            .NotEmpty();
    }
}
