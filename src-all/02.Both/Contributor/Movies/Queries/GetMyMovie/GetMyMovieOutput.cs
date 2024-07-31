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
    public string BudgetDisplayText => Budget.ToMoneyDisplayText();
    public required DateTimeOffset Created { get; init; }
    public required string CreatedBy { get; init; }
    public required MovieStatus Status { get; init; }
    public required DateTimeOffset? Approved { get; init; }
    public required string? ApprovedBy { get; init; }

    public required List<MoviePosterItem> Posters { get; init; } = [];
}

public record MoviePosterItem
{
    public required Guid Id { get; init; }
    public required string Url { get; init; }
    public required string Description { get; init; }
}
