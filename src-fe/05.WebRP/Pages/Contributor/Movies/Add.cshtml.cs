using Delta.Polling.FrontEnd.Logics.Contributor.Movies.Commands.AddMovie;

namespace Delta.Polling.WebRP.Pages.Contributor.Movies;

public class AddModel : PageModelBase
{
    [BindProperty]
    public AddMovieCommand Input { get; set; } = default!;

    public void OnGet()
    {
        Input = new AddMovieCommand
        {
            Title = "Movie 01",
            Storyline = "Storyline Movie 01",
            Budget = 5_000M
        };
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
            Notifier.Success($"Movie {Input.Title} has been added successfully.");

            return RedirectToPage("Details", new { response.Result.Data.MovieId });
        }

        return Page();
    }
}
