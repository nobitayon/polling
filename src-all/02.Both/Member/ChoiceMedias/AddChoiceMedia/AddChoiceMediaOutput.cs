namespace Delta.Polling.Both.Member.ChoiceMedias.AddChoiceMedia;

public record AddChoiceMediaOutput : Output<AddChoiceMediaResult>
{
}

public record AddChoiceMediaResult
{
    public required Guid ChoiceMediaId { get; init; }
}
