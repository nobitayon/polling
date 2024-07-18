using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovies;

namespace Delta.Polling.FrontEnd.Logics.Contributor.Movies.Queries.GetMyMovies;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record GetMyMoviesQuery : GetMyMoviesRequest, IRequest<ResponseResult<GetMyMoviesOutput>>
{
}

public class GetMyMoviesQueryValidator : AbstractValidator<GetMyMoviesQuery>
{
    public GetMyMoviesQueryValidator()
    {
        Include(new GetMyMoviesRequestValidator());
    }
}

public class GetMyMoviesQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetMyMoviesQuery, ResponseResult<GetMyMoviesOutput>>
{
    public async Task<ResponseResult<GetMyMoviesOutput>> Handle(GetMyMoviesQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("Contributor/Movies", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetMyMoviesOutput>(restRequest, cancellationToken);
    }
}
