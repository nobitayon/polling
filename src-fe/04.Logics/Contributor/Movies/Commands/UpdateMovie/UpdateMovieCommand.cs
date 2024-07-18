using Delta.Polling.Both.Contributor.Movies.Commands.UpdateMovie;

namespace Delta.Polling.FrontEnd.Logics.Contributor.Movies.Commands.UpdateMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record UpdateMovieCommand : UpdateMovieRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        Include(new UpdateMovieRequestValidator());
    }
}

public class UpdateMovieCommandHandler(IBackEndService backEndService)
    : IRequestHandler<UpdateMovieCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Contributor/Movies/{request.MovieId}", Method.Patch);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
