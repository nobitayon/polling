using Delta.Polling.Both.Member.ChoiceMedias.AddChoiceMedia;
using Delta.Polling.Logics.Member.ChoiceMedias.AddChoiceMedia;

namespace Delta.Polling.WebAPI.Controllers.Members;

[Route("api/Member/[controller]")]
public class ChoiceMediasController : ApiControllerBase
{
    [HttpPost]
    public async Task<AddChoiceMediaOutput> AddChoiceMedia([FromForm] AddChoiceMediaCommand command)
    {
        return await Sender.Send(command);
    }

    //[HttpGet("Choices/{choiceId}")]
    //public async Task<GetMoviePostersOutput> GetMoviePosters([FromRoute] Guid movieId)
    //{
    //    return await Sender.Send(new GetMoviePostersQuery
    //    {
    //        MovieId = movieId
    //    });
    //}

    //[HttpGet("Download/{moviePosterId}")]
    //public async Task<FileContentResult> GetMoviePosterFile([FromRoute] Guid moviePosterId)
    //{
    //    var response = await Sender.Send(new GetMoviePosterFileQuery
    //    {
    //        MoviePosterId = moviePosterId
    //    });

    //    return response.ToFileContentResult();
    //}
}
