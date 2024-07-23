using Delta.Polling.Both.Member.Polls.Queries.GetPoll;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPoll;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid PollId { get; init; }

    [BindProperty]
    public AddAnotherChoiceOngoingPollCommand Input { get; set; } = default!;

    public PollItem Poll { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        var response = await Sender.Send(new GetPollQuery { PollId = PollId });

        if (response.Result is not null)
        {
            Poll = response.Result.Data;
        }

        return Page();
    }

    public PartialViewResult OnGetAddAnotherChoice([FromQuery] Guid pollId)
    {
        var input = new AddAnotherChoiceOngoingPollCommand { PollId = pollId, Description = null! };
        return new PartialViewResult
        {
            ViewName = "_AddAnotherChoice",
            ViewData = new ViewDataDictionary<AddAnotherChoiceOngoingPollCommand>(ViewData, input)
        };
    }
}
