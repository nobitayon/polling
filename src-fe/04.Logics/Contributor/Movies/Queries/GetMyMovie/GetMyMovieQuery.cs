using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovie;

namespace Delta.Polling.FrontEnd.Logics.Contributor.Movies.Queries.GetMyMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record GetMyMovieQuery : GetMyMovieRequest, IRequest<ResponseResult<GetMyMovieOutput>>
{
}

public class GetMyMovieQueryValidator : AbstractValidator<GetMyMovieQuery>
{
    public GetMyMovieQueryValidator()
    {
        Include(new GetMyMovieRequestValidator());
    }
}

public class GetMyMovieQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetMyMovieQuery, ResponseResult<GetMyMovieOutput>>
{
    public async Task<ResponseResult<GetMyMovieOutput>> Handle(GetMyMovieQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Contributor/Movies/{request.MovieId}", Method.Get);

        return await backEndService.SendRequestAsync<GetMyMovieOutput>(restRequest, cancellationToken);
    }
}
