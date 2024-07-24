using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetMyPolls;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class MyModel(PagerService pagerService) : PageModelBase
{
    public IEnumerable<PollItemModel> Polls { get; set; } = [];
    public string Paging { get; set; } = string.Empty;

    public async Task<IActionResult> OnGet(int? p, int? ps)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetMyPollsQuery
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
            Polls = response.Result.Data.Items.Select(item => new PollItemModel
            {
                Id = item.Id,
                Status = item.Status,
                Title = item.Title
            }).ToList();

            Paging = pagerService.GetHtml("", response.Result.Data.TotalCount, query);
        }

        return Page();
    }
}

public record PollItemModel : PollItem
{
    public string StatusColor
    {
        get
        {
            return Status switch
            {
                PollStatus.Draft => "bg-secondary",
                PollStatus.Ongoing => "bg-warning",
                PollStatus.Finished => "bg-success",
                _ => "bg-primary",
            };
        }
    }
}
