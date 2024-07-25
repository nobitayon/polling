using Delta.Polling.Both.Member.Polls.Queries.GetPoll;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddChoices;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPoll;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid PollId { get; init; }

    [BindProperty]
    public AddAnotherChoiceOngoingPollCommand Input { get; set; } = default!;

    [BindProperty]
    public AddChoiceCommand InputAddChoiceCommand { get; set; } = default!;

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
            ViewName = "~/Pages/Member/Polls/PartialCustom/_AddAnotherChoice.cshtml",
            ViewData = new ViewDataDictionary<AddAnotherChoiceOngoingPollCommand>(ViewData, input)
        };
    }

    public async Task<IActionResult> OnPostAddAnotherChoice(AddAnotherChoiceOngoingPollCommand input)
    {
        Console.WriteLine("input");
        Console.WriteLine(input.PollId);
        Console.WriteLine(input.Description);
        if (ModelState.IsValid)
        {
            var response = await Sender.Send(input);

            if (response.Error is not null)
            {
                Error = response.Error;
                return Page();
            }

            if (response.Result is not null)
            {
                Console.WriteLine(response.Result.Data);
            }

            return new JsonResult(new { isValid = true });
        }
        else
        {
            return new JsonResult(new { isValid = false });
        }
    }

    public async Task<IActionResult> OnPostAddChoiceAsync()
    {
        var response = await Sender.Send(InputAddChoiceCommand);

        if (response.Error is not null)
        {
            Error = response.Error;

            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Add Choice";

            return RedirectToPage("/Member/Polls/Details", new { pollId = PollId });
        }
        else
        {

            TempData["failed"] = "Failed to Add Choice";

            return Page();
        }
    }
}
