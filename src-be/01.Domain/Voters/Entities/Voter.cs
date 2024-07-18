using Delta.Polling.Domain.Abstracts;
using Delta.Polling.Domain.Answers.Entities;
using Delta.Polling.Domain.Polls.Entities;

namespace Delta.Polling.Domain.Voters.Entities;

public record Voter : CreatableEntity
{
    public required Guid PollId { get; init; }
    public Poll Poll { get; set; } = default!;
    public required string Username { get; set; }

    public List<Answer> Answers { get; init; } = [];
}
