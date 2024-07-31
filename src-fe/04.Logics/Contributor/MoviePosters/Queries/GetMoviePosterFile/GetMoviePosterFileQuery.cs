using Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosterFile;

namespace Delta.Polling.FrontEnd.Logics.Contributor.MoviePosters.Queries.GetMoviePosterFile;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record GetMoviePosterFileQuery : GetMoviePosterFileRequest, IRequest<ResponseResult<GetMoviePosterFileOutput>>
{
}

public class GetMoviePosterFileQueryValidator : AbstractValidator<GetMoviePosterFileQuery>
{
    public GetMoviePosterFileQueryValidator()
    {
        Include(new GetMoviePosterFileRequestValidator());
    }
}

public class GetMoviePosterFileQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetMoviePosterFileQuery, ResponseResult<GetMoviePosterFileOutput>>
{
    public async Task<ResponseResult<GetMoviePosterFileOutput>> Handle(GetMoviePosterFileQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Contributor/MoviePosters/Download/{request.MoviePosterId}", Method.Get);

        return await backEndService.SendRequestAsync<GetMoviePosterFileOutput>(restRequest, cancellationToken);
    }
}
