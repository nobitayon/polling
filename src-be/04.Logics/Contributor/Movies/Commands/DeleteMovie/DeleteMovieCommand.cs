using Delta.Polling.Both.Contributor.Movies.Commands.DeleteMovie;

namespace Delta.Polling.Logics.Contributor.Movies.Commands.DeleteMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record DeleteMovieCommand : DeleteMovieRequest, IRequest
{
}

public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
        Include(new DeleteMovieRequestValidator());
    }
}

public class DeleteMovieCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteMovieCommand>
{
    public async Task Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = databaseService.Movies
              .SingleOrDefault(movie => movie.Id == request.MovieId)
              ?? throw new Exception($"Movie with Id {request.MovieId} not found");

        if (movie.CreatedBy != currentUserService.Username)
        {
            throw new Exception($"You cannot delete Movie with Id {request.MovieId} because it is not yours.");
        }

        _ = databaseService.Movies.Remove(movie);
        _ = await databaseService.SaveAsync(cancellationToken);
    }
}
