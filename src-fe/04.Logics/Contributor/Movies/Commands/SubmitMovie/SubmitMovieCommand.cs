using Delta.Polling.Both.Contributor.Movies.Commands.SubmitMovie;

namespace Delta.Polling.FrontEnd.Logics.Contributor.Movies.Commands.SubmitMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record SubmitMovieCommand : SubmitMovieRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class SubmitMovieCommandValidator : AbstractValidator<SubmitMovieCommand>
{
    public SubmitMovieCommandValidator()
    {
        Include(new SubmitMovieRequestValidator());
    }
}

public class SubmitMovieCommandHandler(IBackEndService backEndService)
    : IRequestHandler<SubmitMovieCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(SubmitMovieCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Contributor/Movies/{request.MovieId}/Submit", Method.Post);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
