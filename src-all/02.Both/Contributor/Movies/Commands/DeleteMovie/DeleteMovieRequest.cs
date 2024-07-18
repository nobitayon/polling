namespace Delta.Polling.Both.Contributor.Movies.Commands.DeleteMovie;

public record DeleteMovieRequest
{
    public required Guid MovieId { get; init; }
}

public class DeleteMovieRequestValidator : AbstractValidator<DeleteMovieRequest>
{
    public DeleteMovieRequestValidator()
    {
        _ = RuleFor(x => x.MovieId)
            .NotEmpty();
    }
}
