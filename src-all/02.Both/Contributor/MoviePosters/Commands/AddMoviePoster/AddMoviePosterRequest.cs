namespace Delta.Polling.Both.Contributor.MoviePosters.Commands.AddMoviePoster;

public abstract record AddMoviePosterRequest : FileRequest
{
    public required Guid MovieId { get; init; }
    public required string Description { get; set; }
}

public class AddMoviePosterRequestValidator : AbstractValidator<AddMoviePosterRequest>
{
    public AddMoviePosterRequestValidator()
    {
        Include(new FileRequestValidator());
    }
}
