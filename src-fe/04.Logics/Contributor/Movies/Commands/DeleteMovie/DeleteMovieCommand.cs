using Delta.Polling.Both.Contributor.Movies.Commands.DeleteMovie;

namespace Delta.Polling.FrontEnd.Logics.Contributor.Movies.Commands.DeleteMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record DeleteMovieCommand : DeleteMovieRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
        Include(new DeleteMovieRequestValidator());
    }
}

public class DeleteMovieCommandHandler(IBackEndService backEndService)
    : IRequestHandler<DeleteMovieCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Contributor/Movies/{request.MovieId}", Method.Delete);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
