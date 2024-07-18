namespace Delta.Polling.Both.Contributor.Movies.Commands.UpdateMovie;

public record UpdateMovieRequest
{
    public required Guid MovieId { get; init; }
    public required string Title { get; set; }
    public required string Storyline { get; set; }
    public required decimal Budget { get; set; }
}

public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieRequestValidator()
    {
        _ = RuleFor(x => x.MovieId)
            .NotEmpty();

        _ = RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(MoviesMaxLengthFor.Title);

        _ = RuleFor(x => x.Storyline)
            .NotEmpty()
            .MaximumLength(MoviesMaxLengthFor.Storyline);

        _ = RuleFor(x => x.Budget)
            .InclusiveBetween(MoviesMinValueFor.Budget, MoviesMaxValueFor.Budget);
    }
}
