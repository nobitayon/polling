using Delta.Polling.Both.Member.Polls.Queries.GetOngoingPolls;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetRecentParticipatedPoll;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Member;

public class IndexModel() : PageModelBase
{
    public IEnumerable<PollItem> Polls { get; set; } = [];
    public string Paging { get; set; } = string.Empty;

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

    public async Task<IActionResult> OnGetRecentFinishedPollAsync()
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
                ViewName = "~/Pages/Member/PartialCustom/_RecentPollWithClosed.cshtml",
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
                ViewName = "~/Pages/Member/PartialCustom/_RecentPollWithClosed.cshtml",
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
            ViewName = "~/Pages/Member/PartialCustom/_RecentPollWithClosed.cshtml",
            ViewData = viewDataSuccess
        };
    }
}
