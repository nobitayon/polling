namespace Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovies;

public record GetMyMoviesOutput : Output<PaginatedListResponse<MovieItem>>
{
}

public record MovieItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required decimal Budget { get; init; }
    public string BudgetDisplayText => Budget.ToMoneyDisplayText();
    public required MovieStatus Status { get; init; }
}
