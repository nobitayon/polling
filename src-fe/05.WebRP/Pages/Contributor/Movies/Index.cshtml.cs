using Delta.Polling.FrontEnd.Logics.Contributor.Movies.Queries.GetMyMovies;
using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovies;

namespace Delta.Polling.WebRP.Pages.Contributor.Movies;

public class IndexModel : PageModelBase
{
    public IEnumerable<MovieItem> Movies { get; set; } = [];

    public async Task<IActionResult> OnGet()
    {
        var query = new GetMyMoviesQuery
        {
            MaxCount = 10
        };

        var response = await Sender.Send(query);

        if (response.Result is not null)
        {
            Movies = response.Result.Data;
        }

        return Page();
    }
}
