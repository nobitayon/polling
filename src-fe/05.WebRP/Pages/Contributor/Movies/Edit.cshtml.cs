using Delta.Polling.FrontEnd.Logics.Contributor.Movies.Commands.UpdateMovie;
using Delta.Polling.FrontEnd.Logics.Contributor.Movies.Queries.GetMyMovie;

namespace Delta.Polling.WebRP.Pages.Contributor.Movies;

public class EditModel : PageModelBase
{
    [BindProperty]
    public UpdateMovieCommand Input { get; set; } = default!;

    public async Task<IActionResult> OnGet(Guid movieId)
    {
        var response = await Sender.Send(new GetMyMovieQuery { MovieId = movieId });

        if (response.Result is not null)
        {
            var movie = response.Result.Data;

            Input = new UpdateMovieCommand
            {
                MovieId = movie.Id,
                Title = movie.Title,
                Storyline = movie.Storyline,
                Budget = movie.Budget
            };
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        _ = await Sender.Send(Input);

        return RedirectToPage("Details", new { Input.MovieId });
    }
}
