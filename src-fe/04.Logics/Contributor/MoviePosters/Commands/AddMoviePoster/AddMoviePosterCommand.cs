using Delta.Polling.Both.Contributor.MoviePosters.Commands.AddMoviePoster;

namespace Delta.Polling.FrontEnd.Logics.Contributor.MoviePosters.Commands.AddMoviePoster;

public record AddMoviePosterCommand : AddMoviePosterRequest, IRequest<ResponseResult<AddMoviePosterOutput>>
{
}

public class AddMoviePosterCommandValidator : AbstractValidator<AddMoviePosterCommand>
{
    public AddMoviePosterCommandValidator()
    {
        Include(new AddMoviePosterRequestValidator());
    }
}

public class AddMoviePosterCommandHandler(IBackEndService backEndService)
    : IRequestHandler<AddMoviePosterCommand, ResponseResult<AddMoviePosterOutput>>
{
    public async Task<ResponseResult<AddMoviePosterOutput>> Handle(AddMoviePosterCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("Contributor/MoviePosters", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<AddMoviePosterOutput>(restRequest, cancellationToken);
    }
}
