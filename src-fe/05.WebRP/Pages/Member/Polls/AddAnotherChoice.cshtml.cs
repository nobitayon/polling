using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class AddAnotherChoiceModel : PageModelBase
{
    [BindProperty]
    public AddAnotherChoiceOngoingPollCommand Input { get; set; } = default!;

    public IActionResult OnGet(Guid pollId)
    {

        Input = new AddAnotherChoiceOngoingPollCommand
        {
            PollId = pollId,
            Description = null!
        };

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var response = await Sender.Send(Input);

        if (response.Problem is not null)
        {
            Problem = response.Problem;

            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Another Option successfully added";

            return RedirectToPage("/Member/Polls/Details", new { pollId = Input.PollId });
        }
        else
        {

            TempData["failed"] = "Failed to add another option";

            return Page();
        }
    }
}
