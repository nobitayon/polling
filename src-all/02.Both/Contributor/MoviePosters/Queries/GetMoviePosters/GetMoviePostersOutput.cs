namespace Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosters;

public record GetMoviePostersOutput : Output<MovieItem>
{
}

public record MovieItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required MovieStatus Status { get; init; }
    public required DateTimeOffset Created { get; init; }
    public required string CreatedBy { get; init; }

    public required List<MoviePosterItem> Posters { get; init; } = [];
}

public record MoviePosterItem
{
    public required Guid Id { get; init; }
    public required string Url { get; init; }
    public required string FileName { get; init; }
    public required string Description { get; init; }
}
