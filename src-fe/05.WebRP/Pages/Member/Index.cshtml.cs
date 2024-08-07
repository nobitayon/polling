using System.Text;
using Delta.Polling.Both.Common.Enums;
using Delta.Polling.Both.Member.Polls.Queries.GetOngoingPolls;
using Delta.Polling.Both.Member.Polls.Queries.GetRecentStatistics;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetRecentGeneral;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetRecentParticipatedPoll;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Member;

public class IndexModel() : PageModelBase
{
    public IEnumerable<PollItem> Polls { get; set; } = [];
    public string Paging { get; set; } = string.Empty;

    [BindProperty]
    public StatisticsUser StatUser { get; set; } = default!;

    //public async Task<IActionResult> OnGet()
    //{
    //    var response = await Sender.Send(new GetRecentStatisticsQuery { });

    //    if (response.Problem is not null)
    //    {
    //        Problem = response.Problem;

    //        return Page();
    //    }

    //    if (response.Result is null)
    //    {
    //        Problem = new ProblemDetails { Title = "Ooops something is wring", Detail = "Error get page" };

    //        return Page();
    //    }

    //    StatUser = response.Result.Data;

    //    return Page();
    //}

    public async Task<IActionResult> OnGetRecentPollAsync()
    {
        var response = await Sender.Send(new GetRecentParticipatedPollQuery { });

        if (response.Problem != null)
        {
            var viewData = new ViewDataDictionary(ViewData)
            {
                ["Error"] = response.Problem.Detail,
                ["Data"] = null
            };

            return new PartialViewResult
            {
                ViewName = "~/Pages/Member/PartialCustom/_RecentActivity.cshtml",
                ViewData = viewData
            };
        }

        if (response.Result == null)
        {
            var viewData = new ViewDataDictionary(ViewData)
            {
                ["Error"] = "Failed to get recent activity",
                ["Data"] = null
            };

            return new PartialViewResult
            {
                ViewName = "~/Pages/Member/PartialCustom/_RecentActivity.cshtml",
                ViewData = viewData
            };
        }

        var polls = response.Result.Data;

        var viewDataSuccess = new ViewDataDictionary(ViewData)
        {
            ["Error"] = null,
            ["Data"] = polls
        };

        return new PartialViewResult
        {
            ViewName = "~/Pages/Member/PartialCustom/_RecentActivity.cshtml",
            ViewData = viewDataSuccess
        };
    }

    public async Task<IActionResult> OnGetRecentGeneralAsync()
    {
        var response = await Sender.Send(new GetRecentGeneralQuery { Page = 1, PageSize = 5, SearchField = "Status", SearchText = "Ongoing", MeAlreadyVote = false, SortField = "Modified", SortOrder = SortOrder.Desc });

        if (response.Problem != null)
        {
            var viewData = new ViewDataDictionary(ViewData)
            {
                ["Error"] = response.Problem.Detail,
                ["Data"] = null
            };

            return new PartialViewResult
            {
                ViewName = "~/Pages/Member/PartialCustom/_RecentGeneral.cshtml",
                ViewData = viewData
            };
        }

        if (response.Result == null)
        {
            var viewData = new ViewDataDictionary(ViewData)
            {
                ["Error"] = "Failed to get recent general",
                ["Data"] = null
            };

            return new PartialViewResult
            {
                ViewName = "~/Pages/Member/PartialCustom/_RecentGeneral.cshtml",
                ViewData = viewData
            };
        }

        var polls = response.Result.Data.PollItems.Items;

        var viewDataSuccess = new ViewDataDictionary(ViewData)
        {
            ["Error"] = null,
            ["Data"] = polls
        };

        return new PartialViewResult
        {
            ViewName = "~/Pages/Member/PartialCustom/_RecentGeneral.cshtml",
            ViewData = viewDataSuccess
        };
    }

    public async Task<IActionResult> OnGetJustOneAsync(int p, int s, SortOrder so, string sf, string k, string kf, bool meAlreadyVote)
    {
        var response = await Sender.Send(new GetRecentGeneralQuery
        {
            Page = p,
            PageSize = s,
            MeAlreadyVote = meAlreadyVote,
            SortOrder = so,
            SearchField = kf,
            SearchText = k,
            SortField = sf
        });

        if (response.Problem != null)
        {
            return new JsonResult(new { success = false, Error = response.Problem.Detail });
        }

        if (response.Result == null)
        {
            return new JsonResult(new { success = false, Error = "Failed to get recent general" });
        }

        var polls = response.Result.Data.PollItems.Items;

        var stringBuilder = new StringBuilder();
        foreach (var poll in polls)
        {
            _ = stringBuilder.AppendLine("<div class=\"card mb-3\">");
            _ = stringBuilder.AppendLine("    <div class=\"card-body\">");
            _ = stringBuilder.AppendLine($"        <h5 class=\"card-title\">{poll.Title}</h5>");
            //_ = stringBuilder.AppendLine($"        <p class=\"card-text\">Group: {poll.GroupName}</p>");
            _ = stringBuilder.AppendLine($"        <p class=\"card-text\">{poll.Question}</p>");
            if (poll.Modified != null)
            {
                var modifiedDate = poll.Modified.Value;
                _ = stringBuilder.AppendLine($"        <p class=\"card-text\">Last Modified: {@modifiedDate.ToString("MMM dd, yyyy HH:mm")}</p>");

            }

            _ = stringBuilder.AppendLine($"        <p class=\"card-text\">Created: {poll.Created.ToString("MMM dd, yyyy HH:mm")}</p>");
            _ = stringBuilder.AppendLine($"        <a class=\"btn btn-primary\" href=\"/Member/Polls/Details/{poll.Id}\">View Poll</a>");
            _ = stringBuilder.AppendLine($"        <span class=\"close-btn\" data-id=\"{poll.Id}\">&times;</span>");
            _ = stringBuilder.AppendLine("    </div>");
            _ = stringBuilder.AppendLine("</div>");
        }

        var resultString = stringBuilder.ToString();

        return new JsonResult(new { success = true, Data = resultString });
    }
}
