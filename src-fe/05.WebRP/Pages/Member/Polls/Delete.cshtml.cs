using Delta.Polling.Both.Member.Polls.Queries.GetPoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.DeletePoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPoll;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class DeleteModel : PageModelBase
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

    public async Task<IActionResult> OnPost()
    {
        _ = await Sender.Send(new DeletePollCommand { PollId = PollId });

        return RedirectToPage("/Member/Polls/My");
    }
}
