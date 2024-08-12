using Delta.Polling.Both.Contributor.MoviePosters.Commands.AddMoviePoster;
using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Services.Storage;

namespace Delta.Polling.Logics.Contributor.MoviePosters.Commands.AddMoviePoster;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record AddMoviePosterCommand : AddMoviePosterRequest, IRequest<AddMoviePosterOutput>
{
}

public class AddMoviePosterCommandValidator : AbstractValidator<AddMoviePosterCommand>
{
    public AddMoviePosterCommandValidator()
    {
        Include(new AddMoviePosterRequestValidator());
    }
}

public class AddMoviePosterCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService,
    IStorageService storageService)
    : IRequestHandler<AddMoviePosterCommand, AddMoviePosterOutput>
{
    public async Task<AddMoviePosterOutput> Handle(AddMoviePosterCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new NotAuthenticatedException();
        }

        var movie = await databaseService.Movies
            .AsNoTracking()
            .Where(x => x.Id == request.MovieId)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Movie), request.MovieId);

        if (movie.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException($"You cannot add Movie Poster to Movie with Id {request.MovieId} because the Movie is not created by you.");
        }

        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
        var content = memoryStream.ToArray();

        var moviePosterId = Guid.NewGuid();
        var fileName = $"{moviePosterId}{Path.GetExtension(request.File.FileName)}";
        var storedFileId = await storageService.CreateAsync(content, movie.Id.ToString(), fileName);

        var moviePoster = new MoviePoster
        {
            Id = moviePosterId,
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username,
            MovieId = request.MovieId,
            Description = request.Description,
            FileName = request.File.FileName,
            FileSize = request.File.Length,
            FileContentType = request.File.ContentType,
            StoredFileId = storedFileId
        };

        _ = await databaseService.MoviePosters.AddAsync(moviePoster, cancellationToken);
        _ = await databaseService.SaveAsync(cancellationToken);

        return new AddMoviePosterOutput
        {
            Data = new AddMoviePosterResult
            {
                MoviePosterId = moviePoster.Id
            }
        };
    }
}
