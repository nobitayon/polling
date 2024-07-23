using Delta.Polling.Logics.Contributor.Movies.Commands.AddMovie;
using Delta.Polling.Logics.Contributor.Movies.Commands.DeleteMovie;
using Delta.Polling.Logics.Contributor.Movies.Commands.SubmitMovie;
using Delta.Polling.Logics.Contributor.Movies.Commands.UpdateMovie;
using Delta.Polling.Logics.Contributor.Movies.Queries.GetMyMovie;
using Delta.Polling.Logics.Contributor.Movies.Queries.GetMyMovies;
using Delta.Polling.Both.Contributor.Movies.Commands.AddMovie;
using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovie;
using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovies;

namespace Delta.Polling.WebAPI.Controllers.Contributors;

[Route("api/Contributor/[controller]")]
public class MoviesController : ApiControllerBase
{
    [HttpGet]
    public async Task<GetMyMoviesOutput> GetMyMovies([FromQuery] GetMyMoviesQuery request)
    {
        Console.WriteLine($"{request.Page}");

        var requestTidakValid = new GetMyMoviesQuery
        {
            Page = -2,
            PageSize = -3,
            SearchField = null,
            SearchText = null,
            SortField = null,
            SortOrder = null
        };

        Console.WriteLine($"{requestTidakValid.Page}");

        return await Sender.Send(request);
    }

    [HttpGet("{movieId}")]
    public async Task<GetMyMovieOutput> GetMyMovie(Guid movieId)
    {
        return await Sender.Send(new GetMyMovieQuery
        {
            MovieId = movieId
        });
    }

    [HttpPost]
    public async Task<AddMovieOutput> AddMovie([FromForm] AddMovieCommand command)
    {
        return await Sender.Send(command);
    }

    [HttpDelete("{movieId}")]
    public async Task DeleteMovie(Guid movieId)
    {
        await Sender.Send(new DeleteMovieCommand
        {
            MovieId = movieId
        });
    }

    [HttpPatch("{movieId}")]
    public async Task UpdateMovie(Guid movieId, [FromForm] UpdateMovieCommand command)
    {
        if (movieId != command.MovieId)
        {
            throw new MismatchException(nameof(command.MovieId), movieId, command.MovieId);
        }

        await Sender.Send(command);
    }

    [HttpPost("{movieId}/Submit")]
    public async Task SubmitMovie(Guid movieId)
    {
        await Sender.Send(new SubmitMovieCommand
        {
            MovieId = movieId
        });
    }
}
