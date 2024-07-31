using Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosters;

namespace Delta.Polling.FrontEnd.Logics.Contributor.MoviePosters.Queries.GetMoviePosters;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record GetMoviePostersQuery : GetMoviePostersRequest, IRequest<ResponseResult<GetMoviePostersOutput>>
{
}

public class GetMoviePostersQueryValidator : AbstractValidator<GetMoviePostersQuery>
{
    public GetMoviePostersQueryValidator()
    {
        Include(new GetMoviePostersRequestValidator());
    }
}

public class GetMoviePostersQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetMoviePostersQuery, ResponseResult<GetMoviePostersOutput>>
{
    public async Task<ResponseResult<GetMoviePostersOutput>> Handle(GetMoviePostersQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Contributor/MoviePosters/Movies/{request.MovieId}", Method.Get);

        return await backEndService.SendRequestAsync<GetMoviePostersOutput>(restRequest, cancellationToken);
    }
}
