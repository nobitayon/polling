using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Domain.Choices.Entities;
using Delta.Polling.Domain.Groups.Entities;
using Delta.Polling.Domain.Voters.Entities;

namespace Delta.Polling.Domain.Polls.Entities;

public record Poll : ModifiableEntity
{
    public required Guid GroupId { get; init; }
    public Group Group { get; set; } = default!;
    public required string Title { get; set; }
    public required string Question { get; set; }
    public required int MaximumAnswer { get; set; }
    public required bool AllowOtherChoice { get; set; }
    public required PollStatus Status { get; set; }

    // Navigation Property
    public List<Choice> Choices { get; init; } = [];
    public List<Voter> Voters { get; init; } = [];
}

