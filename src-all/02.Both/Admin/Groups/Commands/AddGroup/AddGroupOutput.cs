namespace Delta.Polling.Both.Admin.Groups.Commands.AddGroup;

public record AddGroupOutput : Output<AddGroupResult>
{
}

public record AddGroupResult
{
    public required Guid GroupId { get; init; }
}
