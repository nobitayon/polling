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

        if (response.Error is not null)
        {
            Error = response.Error;

            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Movie was added successfully.";

            return RedirectToPage("Index");
        }
        else
        {

            TempData["failed"] = "Movie was failed to add.";

            return Page();
        }
    }
}
