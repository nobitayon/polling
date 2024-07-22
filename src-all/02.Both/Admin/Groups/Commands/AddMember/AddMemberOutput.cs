namespace Delta.Polling.Both.Admin.Groups.Commands.AddMember;

public record AddMemberOutput : Output<AddMemberResult>
{
}

public record AddMemberResult
{
    public required List<Guid> ListGroupMemberId { get; init; }
}
