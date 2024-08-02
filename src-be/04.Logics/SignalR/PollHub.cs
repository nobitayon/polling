using Microsoft.AspNetCore.SignalR;

namespace Delta.Polling.Logics.SignalR;

public record ChoiceItem
{
    public required Guid ChoiceId { get; init; }
    public required string Description { get; init; }
    public required int NumVote { get; init; }
}

public class PollHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendVote(Guid pollId, string? choiceItem)
    {
        await Clients.All.SendAsync("SendVote", pollId, choiceItem);
    }
}
