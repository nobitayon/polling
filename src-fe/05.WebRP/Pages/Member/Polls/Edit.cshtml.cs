using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.UpdatePoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPoll;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class EditModel : PageModelBase
{
    [BindProperty]
    public UpdatePollCommand Input { get; set; } = default!;

    public async Task<IActionResult> OnGet(Guid pollId)
    {
        var response = await Sender.Send(new GetPollQuery { PollId = pollId });

        if (response.Result is not null)
        {
            var poll = response.Result.Data;

            Input = new UpdatePollCommand
            {
                PollId = poll.Id,
                Title = poll.Title,
                Question = poll.Question,
                AllowOtherChoice = poll.AllowOtherChoice,
                MaximumAnswer = poll.MaximumAnswer
            };
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        Console.WriteLine("Input");
        Console.WriteLine(Input);

        _ = await Sender.Send(Input);

        return RedirectToPage("Details", new { Input.PollId });
    }
}
