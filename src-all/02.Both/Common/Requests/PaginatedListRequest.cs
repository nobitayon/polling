using Delta.Polling.Both.Common.Enums;

namespace Delta.Polling.Both.Common.Requests;

public class PaginatedListRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchText { get; set; }
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
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        _ = RuleFor(v => v.SortOrder)
            .IsInEnum();
    }
}
