using Delta.Polling.Both.Member.Polls.Queries.GetPoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPoll;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid PollId { get; init; }

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
}
