namespace Delta.Polling.Both.Common.Requests;

public abstract record PaginatedListRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? SearchText { get; set; }
    public string? SearchField { get; set; }
    public string? SortField { get; set; }
    public SortOrder? SortOrder { get; set; }
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
