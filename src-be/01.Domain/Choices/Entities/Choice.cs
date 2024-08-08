using Delta.Polling.Domain.Answers.Entities;
using Delta.Polling.Domain.Polls.Entities;

namespace Delta.Polling.Domain.Choices.Entities;

public record Choice : ModifiableEntity
{
    public required Guid PollId { get; init; }
    public Poll Poll { get; set; } = default!;
    public required string Description { get; set; }
    public required bool IsOther { get; set; }

    public List<Answer> Answers { get; init; } = [];
    public IEnumerable<ChoiceMedia> ChoiceMedias { get; set; } = new List<ChoiceMedia>();
}
