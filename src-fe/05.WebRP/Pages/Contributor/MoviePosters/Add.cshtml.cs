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
        var response = await Sender.Send(new GetMyMovieQuery { MovieId = MovieId });

        if (response.Error is not null)
        {
            Error = response.Error;

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
        Console.WriteLine($"OnPost -- Input.MovieId: {Input.MovieId}");

        var response = await Sender.Send(Input);

        if (response.Error is not null)
        {
            Error = response.Error;

            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Movie Poster was added successfully.";

            return RedirectToPage("/Contributor/Movies/Details", new { MovieId });
        }
        else
        {

            TempData["failed"] = "Movie Poster was failed to add.";

            return Page();
        }
    }
}
