using Delta.Polling.Domain.Abstracts;
using Delta.Polling.Domain.Choices.Entities;
using Delta.Polling.Domain.Voters.Entities;

namespace Delta.Polling.Domain.Answers.Entities;

public record Answer : CreatableEntity
{
    public required Guid ChoiceId { get; init; }
    public Choice Choice { get; set; } = default!;
    public required Guid VoterId { get; init; }
    public Voter Voter { get; set; } = default!;
}
