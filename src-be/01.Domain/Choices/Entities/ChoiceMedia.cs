namespace Delta.Polling.Domain.Choices.Entities;

public record ChoiceMedia : FileEntity
{
    public required Guid ChoiceId { get; init; }
    public Choice Choice { get; init; } = default!;

    public required string Description { get; set; }
}
