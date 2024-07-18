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
            Title = "",
            Storyline = "",
            Budget = 0M
        };
    }

    public async Task<IActionResult> OnPost()
    {
        var response = await Sender.Send(Input);

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
