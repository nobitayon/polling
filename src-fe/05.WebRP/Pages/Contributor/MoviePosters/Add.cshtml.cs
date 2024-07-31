using Delta.Polling.FrontEnd.Logics.Contributor.MoviePosters.Commands.AddMoviePoster;
using Delta.Polling.FrontEnd.Logics.Contributor.Movies.Queries.GetMyMovie;

namespace Delta.Polling.WebRP.Pages.Contributor.MoviePosters;

public class AddModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public required Guid MovieId { get; init; }

    [BindProperty]
    public AddMoviePosterCommand Input { get; set; } = default!;

    public required string MovieTitle { get; set; }

    public async Task OnGetAsync()
    {
        var response = await Sender.Send(new GetMyMovieQuery
        {
            MovieId = MovieId
        });

        if (response.Problem is not null)
        {
            Problem = response.Problem;

            return;
        }

        if (response.Result is not null)
        {
            MovieTitle = response.Result.Data.Title;

            Input = new()
            {
                MovieId = MovieId,
                Description = string.Empty
            };
        }
    }

    public async Task<IActionResult> OnPost()
    {
        var response = await Sender.Send(Input);

        if (response.Problem is not null)
        {
            Problem = response.Problem;

            return Page();
        }

        if (response.Result is not null)
        {
            return RedirectToPage("/Contributor/Movies/Details", new { MovieId });
        }

        return Page();
    }
}
