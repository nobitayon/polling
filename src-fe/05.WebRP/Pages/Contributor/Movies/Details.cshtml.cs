using Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosterFile;
using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovie;
using Delta.Polling.FrontEnd.Logics.Contributor.MoviePosters.Queries.GetMoviePosterFile;
using Delta.Polling.FrontEnd.Logics.Contributor.Movies.Queries.GetMyMovie;

namespace Delta.Polling.WebRP.Pages.Contributor.Movies;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid MovieId { get; init; }

    public MovieItem Movie { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        await LoadData();

        return Page();
    }

    public async Task<IActionResult> OnPostDownloadAsync(Guid moviePosterId)
    {
        await LoadData();

        var output = await GetFile(moviePosterId);

        if (output is null)
        {
            return Page();
        }

        return File(output.Content, output.ContentType, output.FileName);
    }

    private async Task LoadData()
    {
        PageTitle = "Movie Details";

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
            Movie = response.Result.Data;
            PageTitle = Movie.Title;
        }
    }

    private async Task<GetMoviePosterFileOutput?> GetFile(Guid moviePosterId)
    {
        var response = await Sender.Send(new GetMoviePosterFileQuery
        {
            MoviePosterId = moviePosterId
        });

        if (response.Problem is not null)
        {
            Problem = response.Problem;
        }

        return response.Result;
    }
}
