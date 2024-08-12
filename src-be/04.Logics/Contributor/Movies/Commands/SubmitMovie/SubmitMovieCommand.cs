using Delta.Polling.Both.Contributor.Movies.Commands.SubmitMovie;
using Delta.Polling.Domain.Movies.Entities;

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
            ?? throw new EntityNotFoundException(nameof(Movie), request.MovieId);

        if (movie.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException($"You cannot update Movie with Id {request.MovieId} because the Movie is not created by you.");
        }

        if (movie.Status is not MovieStatus.Draft and not MovieStatus.Returned)
        {
            throw new InvalidOperationException($"Movie with Id {request.MovieId} cannot be submitted.");
        }

        movie.Status = MovieStatus.Pending;

        _ = await databaseService.SaveAsync(cancellationToken);
    }
}
