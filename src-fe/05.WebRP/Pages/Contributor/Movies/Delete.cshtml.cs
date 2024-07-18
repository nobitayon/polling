using Delta.Polling.FrontEnd.Logics.Contributor.Movies.Commands.DeleteMovie;
using Delta.Polling.FrontEnd.Logics.Contributor.Movies.Queries.GetMyMovie;
using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovie;

namespace Delta.Polling.WebRP.Pages.Contributor.Movies;

public class DeleteModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid MovieId { get; init; }

    public MovieItem Movie { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        var response = await Sender.Send(new GetMyMovieQuery { MovieId = MovieId });

        if (response.Result is not null)
        {
            Movie = response.Result.Data;
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        _ = await Sender.Send(new DeleteMovieCommand { MovieId = MovieId });

        return RedirectToPage("Index");
    }
}
