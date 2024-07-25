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

        //using var streamReader = new StreamReader(request.File.OpenReadStream());
        //var stringContent = await streamReader.ReadToEndAsync(cancellationToken);
        //var content = Convert.FromBase64String(stringContent);

        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
        var content = memoryStream.ToArray();

        var storedFileId = await storageService.CreateAsync(content);

        var moviePoster = new MoviePoster
        {
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username,
            MovieId = request.MovieId,
            Description = request.Description,
            FileName = request.File.FileName,
            FileSize = request.File.Length,
            FileContentType = request.File.ContentType,
            StoredFileId = storedFileId
        };

        return new AddMoviePosterOutput
        {
            Data = new AddMoviePosterResult
            {
                MoviePosterId = moviePoster.Id
            }
        };
    }
}
