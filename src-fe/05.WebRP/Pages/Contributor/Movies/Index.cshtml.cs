using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovies;
using Delta.Polling.FrontEnd.Logics.Contributor.Movies.Queries.GetMyMovies;

namespace Delta.Polling.WebRP.Pages.Contributor.Movies;

public class IndexModel(PagerService pagerService) : PageModelBase
{
    public IEnumerable<MovieItemModel> Movies { get; set; } = [];
    public string Paging { get; set; } = string.Empty;

    public async Task<IActionResult> OnGet(int? p, int? ps, string? k)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetMyMoviesQuery
        {
            Page = page,
            PageSize = pageSize,
            SearchText = k,
            SortField = null,
            SortOrder = null
        };

        var response = await Sender.Send(query);

        if (response.Result is not null)
        {
            Movies = response.Result.Data.Items.Select(movieItem => new MovieItemModel
            {
                Id = movieItem.Id,
                Title = movieItem.Title,
                Budget = movieItem.Budget,
                Status = movieItem.Status
            });

            Paging = pagerService.GetHtml("Movies", response.Result.Data.TotalCount, query);
        }

        return Page();
    }
}

public record MovieItemModel : MovieItem
{
    public string StatusColor
    {
        get
        {
            return Status switch
            {
                MovieStatus.Draft => "bg-secondary",
                MovieStatus.Pending => "bg-warning",
                MovieStatus.Approved => "bg-success",
                MovieStatus.Returned => "bg-primary",
                MovieStatus.Rejected => "bg-danger",
                _ => "bg-primary",
            };
        }
    }
}
