namespace Delta.Polling.Both.Contributor.Movies.Commands.AddMovie;

public record AddMovieOutput : Output<AddMovieResult>
{
}

public record AddMovieResult
{
    public required Guid MovieId { get; init; }
}
