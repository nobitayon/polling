using Delta.Polling.Both.Contributor.Movies.Commands.SubmitMovie;

namespace Delta.Polling.Logics.Contributor.Movies.Commands.SubmitMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record SubmitMovieCommand : SubmitMovieRequest, IRequest
{
}

public class SubmitMovieCommandValidator : AbstractValidator<SubmitMovieCommand>
{
    public SubmitMovieCommandValidator()
    {
        Include(new SubmitMovieRequestValidator());
    }
}

public class SubmitMovieCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<SubmitMovieCommand>
{
    public async Task Handle(SubmitMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await databaseService.Movies
              .Where(movie => movie.Id == request.MovieId)
              .SingleOrDefaultAsync(cancellationToken)
              ?? throw new Exception($"Movie dengan ID {request.MovieId} tidak dapat ditemukan.");

        if (movie.CreatedBy != currentUserService.Username)
        {
            throw new Exception($"You cannot update Movie with Id {request.MovieId} because it is not yours.");
        }

        if (movie.Status is not MovieStatus.Draft and not MovieStatus.Returned)
        {
            throw new Exception($"Movie with Id {request.MovieId} cannot be submitted.");
        }

        movie.Status = MovieStatus.Pending;

        _ = await databaseService.SaveAsync(cancellationToken);
    }
}
