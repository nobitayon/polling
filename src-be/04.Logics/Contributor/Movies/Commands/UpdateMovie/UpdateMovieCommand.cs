using Delta.Polling.Both.Contributor.Movies.Commands.UpdateMovie;

namespace Delta.Polling.Logics.Contributor.Movies.Commands.UpdateMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record UpdateMovieCommand : UpdateMovieRequest, IRequest
{
}

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        Include(new UpdateMovieRequestValidator());
    }
}

public class UpdateMovieCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateMovieCommand>
{
    public async Task Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await databaseService.Movies
              .Where(movie => movie.Id == request.MovieId)
              .SingleOrDefaultAsync(cancellationToken) ?? throw new Exception($"Movie dengan ID {request.MovieId} tidak dapat ditemukan.");

        if (movie.CreatedBy != currentUserService.Username)
        {
            throw new Exception($"You cannot update Movie with Id {request.MovieId} because it is not yours.");
        }

        if (movie.Status is not MovieStatus.Draft)
        {
            throw new Exception($"Movie with Id {request.MovieId} cannot be updated because its status is not Draft.");
        }

        movie.Title = request.Title;
        movie.Storyline = request.Storyline;
        movie.Budget = request.Budget;

        _ = await databaseService.SaveAsync(cancellationToken);
    }
}
