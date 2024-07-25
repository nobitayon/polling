using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.AddPoll;

namespace Delta.Polling.WebRP.Pages.Member.Groups;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid GroupId { get; init; }

    [BindProperty]
    public AddPollCommand Input { get; set; } = default!;

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAddPollAsync()
    {
        var response = await Sender.Send(Input);

        if (response.Error is not null)
        {
            Error = response.Error;

            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success create poll";

            return RedirectToPage("/Member/Polls/Details", new { pollId = response.Result.Data.PollId });
        }
        else
        {

            TempData["failed"] = "Failed to create poll";

            return Page();
        }
    }
}
