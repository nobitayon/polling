namespace Delta.Polling.Both.Common.Requests;

public abstract record PaginatedListRequest
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public string? SearchText { get; init; }
    public string? SearchField { get; init; }
    public string? SortField { get; init; }
    public SortOrder? SortOrder { get; init; }
}

public class PaginatedListRequestValidator : AbstractValidator<PaginatedListRequest>
{
    public PaginatedListRequestValidator()
    {
        _ = RuleFor(v => v.Page)
            .GreaterThan(0);

        _ = RuleFor(v => v.PageSize)
            .InclusiveBetween(1, 100);

        _ = RuleFor(v => v.SortOrder)
            .IsInEnum();
    }
}
