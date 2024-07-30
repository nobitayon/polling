namespace Delta.Polling.Both.Admin.Groups.Commands.AddMember;

public record AddMemberOutput : Output<AddMemberResult>
{
}

public record AddMemberResult
{
    public required Guid GroupMemberId { get; init; }
}
