using Delta.Polling.Both.Member.Polls.Queries.GetPoll;
using Delta.Polling.FrontEnd.Infrastructure.BackEnd;
using Delta.Polling.FrontEnd.Logics.Member.Choices.Queries.GetChoicesByPoll;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPoll;
using Delta.Polling.FrontEnd.Services.CurrentUser;
using Microsoft.Extensions.Options;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class LiveResultModel(
    ICurrentUserService currentUserService,
    IOptions<BackEndOptions> backEndOptions) : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid PollId { get; init; }

    [BindProperty]
    public PollItem Poll { get; set; } = default!;

    public IActionResult OnGetAccessToken()
    {
        if (currentUserService.AccessToken == null)
        {
            return new JsonResult(new { isValid = false });
        }

        return new JsonResult(new { isValid = true, accessToken = currentUserService.AccessToken, apiBaseUri = backEndOptions.Value.ApiBaseUrl, pollId = PollId });
    }

    public async Task<IActionResult> OnGet()
    {
        var response = await Sender.Send(new GetPollQuery { PollId = PollId });

        if (response.Problem is not null)
        {
            Problem = response.Problem;

            return Page();
        }

        if (response.Result is null)
        {
            Problem = new ProblemDetails { Detail = "Get error fetch poll", Status = 500 };
            return Page();
        }

        Poll = response.Result.Data;

        return Page();
    }

    public async Task<IActionResult> OnGetFirstTime()
    {
        var response = await Sender.Send(new GetChoicesByPollQuery { PollId = PollId });

        if (response.Problem is not null)
        {
            Problem = response.Problem;

            return new JsonResult(new { success = false, problem = Problem });
        }

        if (response.Result is null)
        {
            return new JsonResult(new { success = false });

        }

        return new JsonResult(new { success = true, data = response.Result.Data });
    }
}
