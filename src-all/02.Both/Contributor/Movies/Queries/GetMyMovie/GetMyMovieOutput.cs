namespace Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovie;

public record GetMyMovieOutput : Output<MovieItem>
{
}

public record MovieItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Storyline { get; init; }
    public required decimal Budget { get; init; }
    public required DateTimeOffset Created { get; init; }
    public required MovieStatus Status { get; set; }
    public DateTimeOffset? Approved { get; set; }
    public string? ApprovedBy { get; set; }
}