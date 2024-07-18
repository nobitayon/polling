namespace Delta.Polling.Both.Contributor.Movies.Commands.AddMovie;

public record AddMovieRequest
{
    public required string Title { get; set; }
    public required string Storyline { get; set; }
    public required decimal Budget { get; set; }
}

public class AddMovieRequestValidator : AbstractValidator<AddMovieRequest>
{
    public AddMovieRequestValidator()
    {
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
