using Delta.Polling.Both.Contributor.Movies.Commands.AddMovie;

namespace Delta.Polling.FrontEnd.Logics.Contributor.Movies.Commands.AddMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record AddMovieCommand : AddMovieRequest, IRequest<ResponseResult<AddMovieOutput>>
{
}

public class AddMovieCommandValidator : AbstractValidator<AddMovieCommand>
{
    public AddMovieCommandValidator()
    {
        Include(new AddMovieRequestValidator());
    }
}

public class AddMovieCommandHandler(IBackEndService backEndService)
    : IRequestHandler<AddMovieCommand, ResponseResult<AddMovieOutput>>
{
    public async Task<ResponseResult<AddMovieOutput>> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("Contributor/Movies", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<AddMovieOutput>(restRequest, cancellationToken);
    }
}
