using Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosterFile;
using Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosters;
using Delta.Polling.FrontEnd.Logics.Contributor.MoviePosters.Queries.GetMoviePosterFile;
using Delta.Polling.FrontEnd.Logics.Contributor.MoviePosters.Queries.GetMoviePosters;

namespace Delta.Polling.WebRP.Pages.Contributor.MoviePosters;

public class IndexModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public required Guid MovieId { get; init; }

    public required MovieItem Movie { get; set; }

    public async Task<IActionResult> OnGetAsync()
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
        var response = await Sender.Send(new GetMoviePostersQuery
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
