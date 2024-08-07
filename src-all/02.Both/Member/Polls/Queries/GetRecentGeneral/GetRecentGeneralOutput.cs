using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Both.Member.Polls.Queries.GetRecentGeneral;

public record GetRecentGeneralOutput : Output<GetRecentGeneralResult>
{
}

public record GetRecentGeneralResult
{
    public required PaginatedListResponse<PollItem> PollItems { get; init; }
}

public record PollItem
{
    public required Guid Id { get; init; }
    public required string GroupName { get; set; } = default!;
    public required string Title { get; set; } = default!;
    public required string Question { get; set; } = default!;
    public required PollStatus Status { get; set; } = default!;
    public required DateTimeOffset Created { get; set; } = default!;
    public required string CreatedBy { get; set; } = default!;
    public required DateTimeOffset? Modified { get; set; } = default!;
}
