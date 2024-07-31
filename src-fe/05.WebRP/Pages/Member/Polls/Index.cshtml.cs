using System.Text;
using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Queries.GetOngoingPolls;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetOngoingPolls;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class IndexModel(PagerService pagerService) : PageModelBase
{
    public IEnumerable<OngoingPollItemModel> Polls { get; set; } = [];
    public string Paging { get; set; } = string.Empty;

    public async Task<IActionResult> OnGet(int? p, int ps = 5)
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
            Polls = response.Result.Data.Items.Select(item => new OngoingPollItemModel
            {
                Id = item.Id,
                Status = item.Status,
                Title = item.Title,
                Created = item.Created,
                CreatedBy = item.CreatedBy,
                GroupName = item.GroupName
            }).ToList();

            Paging = pagerService.GetHtml("", response.Result.Data.TotalCount, query);
        }

        return Page();
    }

    public async Task<IActionResult> OnGetLoadMoreOngoingPoll(int? p, int? ps)
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

        if (response.Problem is not null)
        {
            Problem = response.Problem;

            var pagerBuilderLocal = new StringBuilder();

            var newButton = $@"<button data-current-page=""{p - 1}"" id=""load-more-button"" onclick=""handleClickMore()"">Error, try again Load More</button>";
            _ = pagerBuilderLocal.Append(newButton);

            return new JsonResult(new { success = false, html = pagerBuilderLocal.ToString() });
        }

        if (response.Result is null)
        {
            var pagerBuilderLocal = new StringBuilder();

            var newButton = $@"<button data-current-page=""{p - 1}"" id=""load-more-button"" onclick=""handleClickMore()"">Error, try again Load More</button>";
            _ = pagerBuilderLocal.Append(newButton);

            return new JsonResult(new { success = false, html = pagerBuilderLocal.ToString() });
        }

        var polls = response.Result.Data.Items.Select(item => new OngoingPollItemModel
        {
            Id = item.Id,
            Status = item.Status,
            Title = item.Title,
            Created = item.Created,
            CreatedBy = item.CreatedBy,
            GroupName = item.GroupName
        }).ToList();

        if (polls.Count == 0)
        {
            var pagerBuilderLocal = new StringBuilder();

            var newButton = $@"<div>No more ongoing poll</button>";
            _ = pagerBuilderLocal.Append(newButton);

            return new JsonResult(new { success = true, html = pagerBuilderLocal.ToString() });
        }

        var pagerBuilder = new StringBuilder();
        foreach (var poll in polls)
        {
            var multilineText = $@"
<div class=""poll-card"">
    <div class=""poll-card-header"">
        <h5 class=""mb-0"">{poll.Title}</h5>
        <p class=""text-muted mb-0"">Posted by {poll.CreatedBy} on {poll.Created:MMM dd, yyyy HH:mm}</p>
        <p class=""text-muted mb-0""><small>Group: {poll.GroupName}</small></p>
    </div>
    <div class=""poll-card-body"">
        <a href=""/Member/Polls/Details/{poll.Id}"" class=""btn btn-primary btn-view"">View Poll</a>
    </div>
</div>";

            _ = pagerBuilder.Append(multilineText);
        }

        var newButtonResult = $@"<button data-current-page=""{p}"" id=""load-more-button"">Load More</button>";
        _ = pagerBuilder.Append(newButtonResult);

        return new JsonResult(new { success = true, html = pagerBuilder.ToString() });

    }
}

public record OngoingPollItemModel : PollItem
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
