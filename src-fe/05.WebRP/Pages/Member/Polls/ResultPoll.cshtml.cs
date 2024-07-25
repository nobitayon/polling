using Delta.Polling.Both.Member.Polls.Queries.GetPollWithAllAnswer;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPollWithAllAnswerQuery;
namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class ResultPollModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid PollId { get; init; }

    [BindProperty]
    public GetPollWithAllAnswerQuery Input { get; set; } = default!;

    public PollItem Poll { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        var response = await Sender.Send(new GetPollWithAllAnswerQuery { PollId = PollId });

        if (response.Result is not null)
        {
            Poll = response.Result.Data;

            Console.WriteLine($"Hello Poll answer detail ${Poll.Question}");
        }

        return Page();
    }
}
