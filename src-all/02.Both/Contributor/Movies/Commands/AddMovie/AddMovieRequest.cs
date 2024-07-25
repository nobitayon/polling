namespace Delta.Polling.Both.Contributor.Movies.Commands.AddMovie;

public abstract record AddMovieRequest
{
    public required string Title { get; set; }
    public required string Storyline { get; set; }
    public required decimal Budget { get; set; }
}

public class AddMovieRequestValidator : AbstractValidator<AddMovieRequest>
{
    public AddMovieRequestValidator()
    {
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
