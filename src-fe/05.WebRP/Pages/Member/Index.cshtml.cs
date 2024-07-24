using Delta.Polling.Both.Member.Polls.Queries.GetOngoingPolls;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetOngoingPolls;

namespace Delta.Polling.WebRP.Pages.Member;

public class IndexModel(PagerService pagerService) : PageModelBase
{
    public IEnumerable<PollItem> Polls { get; set; } = [];
    public string Paging { get; set; } = string.Empty;

    public async Task<IActionResult> OnGet(int? p, int? ps)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetOngoingPollsQuery
        {
            Page = page,
            PageSize = pageSize,
            SearchText = null,
            SortField = null,
            SortOrder = null
        };

        var response = await Sender.Send(query);

        if (response.Result is not null)
        {
            Polls = response.Result.Data.Items.ToList();

            Paging = pagerService.GetHtml("Home", response.Result.Data.TotalCount, query);
        }

        return Page();
    }
}
