using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.AddPoll;
using Delta.Polling.FrontEnd.Logics.Member.Groups.Queries.GetMyGroup;
using Delta.Polling.Both.Member.Groups.Queries.GetMyGroup;

namespace Delta.Polling.WebRP.Pages.Member.Groups;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid GroupId { get; init; }

    public GroupItem GroupItem { get; set; } = default!;

    [BindProperty]
    public AddPollCommand Input { get; set; } = default!;

    public async Task<IActionResult> OnGet(int? p, int ps = 5)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetMyGroupQuery
        {
            GroupId = GroupId,
            Page = page,
            PageSize = pageSize,
            SearchText = null,
            SortField = null,
            SortOrder = null
        };

        var response = await Sender.Send(query);

        if (response.Error is not null)
        {
            Error = response.Error;

            return Page();
        }

        if (response.Result is null)
        {
            TempData["failed"] = "failed get poll";

            return Page();
        }

        GroupItem = response.Result.Data;

        return Page();
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
