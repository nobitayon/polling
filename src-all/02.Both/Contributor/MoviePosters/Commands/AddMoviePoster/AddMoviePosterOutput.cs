namespace Delta.Polling.Both.Contributor.MoviePosters.Commands.AddMoviePoster;

public record AddMoviePosterOutput : Output<AddMoviePosterResult>
{
}

public record AddMoviePosterResult
{
    public required Guid MoviePosterId { get; init; }
}
