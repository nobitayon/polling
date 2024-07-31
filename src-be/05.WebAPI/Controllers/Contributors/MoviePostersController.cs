using Delta.Polling.Both.Contributor.MoviePosters.Commands.AddMoviePoster;
using Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosters;
using Delta.Polling.Logics.Contributor.MoviePosters.Commands.AddMoviePoster;
using Delta.Polling.Logics.Contributor.MoviePosters.Queries.GetMoviePosterFile;
using Delta.Polling.Logics.Contributor.MoviePosters.Queries.GetMoviePosters;

namespace Delta.Polling.WebAPI.Controllers.Contributors;

[Route("api/Contributor/[controller]")]
public class MoviePostersController : ApiControllerBase
{
    [HttpPost]
    public async Task<AddMoviePosterOutput> AddMoviePoster([FromForm] AddMoviePosterCommand command)
    {
        return await Sender.Send(command);
    }

    [HttpGet("Movies/{movieId}")]
    public async Task<GetMoviePostersOutput> GetMoviePosters([FromRoute] Guid movieId)
    {
        return await Sender.Send(new GetMoviePostersQuery
        {
            MovieId = movieId
        });
    }

    [HttpGet("Download/{moviePosterId}")]
    public async Task<FileContentResult> GetMoviePosterFile([FromRoute] Guid moviePosterId)
    {
        var response = await Sender.Send(new GetMoviePosterFileQuery
        {
            MoviePosterId = moviePosterId
        });

        return response.ToFileContentResult();
    }
}
