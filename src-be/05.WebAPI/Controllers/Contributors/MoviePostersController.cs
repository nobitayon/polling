using Delta.Polling.Both.Contributor.MoviePosters.Commands.AddMoviePoster;
using Delta.Polling.Logics.Contributor.MoviePosters.Commands.AddMoviePoster;

namespace Delta.Polling.WebAPI.Controllers.Contributors;

[Route("api/Contributor/[controller]")]
public class MoviePostersController : ApiControllerBase
{
    [HttpPost]
    public async Task<AddMoviePosterOutput> AddMoviePoster([FromForm] AddMoviePosterCommand command)
    {
        return await Sender.Send(command);
    }
}
