namespace Delta.Polling.Both.Contributor.Movies.Commands.UpdateMovie;

public abstract record UpdateMovieRequest
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
        _ = RuleFor(input => input.MovieId)
            .NotEmpty();

        _ = RuleFor(input => input.Title)
            .NotEmpty()
            .MaximumLength(MoviesMaxLengthFor.Title);

        _ = RuleFor(input => input.Storyline)
            .NotEmpty()
            .MaximumLength(MoviesMaxLengthFor.Storyline);

        _ = RuleFor(input => input.Budget)
            .InclusiveBetween(MoviesMinValueFor.Budget, MoviesMaxValueFor.Budget);
    }
}
