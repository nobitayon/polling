using Delta.Polling.Both.Member.Polls.Commands.AddPoll;
using Delta.Polling.Both.Member.Polls.Commands.AddVote;
using Delta.Polling.Both.Member.Polls.Commands.UpdateVote;
using Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;
using Delta.Polling.Both.Member.Polls.Queries.GetPoll;
using Delta.Polling.Logics.Member.Polls.Commands.AddPoll;
using Delta.Polling.Logics.Member.Polls.Commands.AddVote;
using Delta.Polling.Logics.Member.Polls.Commands.UpdateVote;
using Delta.Polling.Logics.Member.Polls.Commands.UpdatePoll;
using Delta.Polling.Logics.Member.Polls.Queries.GetMyPolls;
using Delta.Polling.Logics.Member.Polls.Queries.GetPoll;
using Delta.Polling.Logics.Member.Polls.Commands.StartPoll;
using Delta.Polling.Logics.Member.Polls.Commands.FinishPoll;
using Delta.Polling.Both.Member.Polls.Queries.GetOngoingPolls;
using Delta.Polling.Logics.Member.Polls.Queries.GetOngoingPolls;
using Delta.Polling.Logics.Member.Polls.Commands.DeletePoll;
using Delta.Polling.Logics.Member.Polls.Queries.GetPollWithAllAnswer;
using Delta.Polling.Both.Member.Polls.Queries.GetPollWithAllAnswer;

namespace Delta.Polling.WebAPI.Controllers.Members;

[Route("api/Member/[controller]")]
public class PollsController : ApiControllerBase
{
    [HttpGet]
    public async Task<GetMyPollsOutput> GetMyPolls([FromQuery] GetMyPollsQuery request)
    {
        return await Sender.Send(request);
    }

    [HttpGet("ongoing")]
    public async Task<GetOngoingPollsOutput> GetOngoingPolls([FromQuery] GetOngoingPollsQuery request)
    {
        return await Sender.Send(request);
    }

    [HttpGet("{pollId:guid}")]
    public async Task<GetPollOutput> GetPoll([FromRoute] Guid pollId)
    {
        var request = new GetPollQuery { PollId = pollId };

        return await Sender.Send(request);
    }

    [HttpPost]
    public async Task<AddPollOutput> AddPoll([FromForm] AddPollCommand request)
    {
        return await Sender.Send(request);
    }

    // TODO: redundant pollid
    [HttpPut("{pollId:guid}")]
    public async Task UpdatePoll([FromRoute] Guid pollId, [FromForm] UpdatePollCommand request)
    {
        if (pollId != request.PollId)
        {
            throw new MismatchException(nameof(request.PollId), pollId, request.PollId);
        }

        await Sender.Send(request);
    }

    // TODO: redundant pollid
    [HttpPost("{pollId:guid}/vote")]
    public async Task<AddVoteOutput> SubmitVote([FromRoute] Guid pollId, [FromForm] AddVoteCommand request)
    {
        if (pollId != request.PollId)
        {
            throw new MismatchException(nameof(request.PollId), pollId, request.PollId);
        }

        return await Sender.Send(request);
    }

    // TODO: redundant pollid
    [HttpPost("{pollId:guid}/update-vote")]
    public async Task<UpdateVoteOutput> UpdateVote([FromRoute] Guid pollId, [FromForm] UpdateVoteCommand request)
    {
        if (pollId != request.PollId)
        {
            throw new MismatchException(nameof(request.PollId), pollId, request.PollId);
        }

        return await Sender.Send(request);
    }

    [HttpPatch("{pollId:guid}/start")]
    public async Task StartPoll([FromRoute] StartPollCommand request)
    {
        await Sender.Send(request);
    }

    [HttpPost("{pollId:guid}/finish")]
    public async Task FinishPoll([FromRoute] FinishPollCommand request)
    {
        await Sender.Send(request);
    }

    [HttpDelete("{pollId:guid}")]
    public async Task DeletePoll([FromRoute] DeletePollCommand request)
    {
        await Sender.Send(request);
    }

    [HttpGet("{pollId:guid}/answer-detail")]
    public async Task<GetPollWithAllAnswerOutput> GetAllAnswerPoll([FromRoute] GetPollWithAllAnswerQuery request)
    {
        return await Sender.Send(request);
    }
}
