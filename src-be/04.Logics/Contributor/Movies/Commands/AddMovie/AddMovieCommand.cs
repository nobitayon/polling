using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Both.Contributor.Movies.Commands.AddMovie;

namespace Delta.Polling.Logics.Contributor.Movies.Commands.AddMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record AddMovieCommand : AddMovieRequest, IRequest<AddMovieOutput>
{
}

public class AddMovieCommandValidator : AbstractValidator<AddMovieCommand>
{
    public AddMovieCommandValidator()
    {
        Include(new AddMovieRequestValidator());
    }
}

public class AddMovieCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddMovieCommand, AddMovieOutput>
{
    public async Task<AddMovieOutput> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var movie = new Movie
        {
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username,
            Title = request.Title,
            Storyline = request.Storyline,
            Budget = request.Budget,
            Status = MovieStatus.Draft
        };

        _ = await databaseService.Movies.AddAsync(movie, cancellationToken);

        _ = await databaseService.SaveAsync(cancellationToken);

        var result = new AddMovieResult
        {
            MovieId = movie.Id
        };

        return new AddMovieOutput
        {
            Data = result
        };
    }
}
