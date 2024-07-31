using Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosterFile;
using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Services.Storage;

namespace Delta.Polling.Logics.Contributor.MoviePosters.Queries.GetMoviePosterFile;

public record GetMoviePosterFileQuery : GetMoviePosterFileRequest, IRequest<GetMoviePosterFileOutput>
{
}

public class GetMoviePosterFileQueryValidator : AbstractValidator<GetMoviePosterFileQuery>
{
    public GetMoviePosterFileQueryValidator()
    {
        Include(new GetMoviePosterFileRequestValidator());
    }
}

public class GetMoviePosterFileQueryHandler(
    IDatabaseService databaseService,
    IStorageService storageService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMoviePosterFileQuery, GetMoviePosterFileOutput>
{
    public async Task<GetMoviePosterFileOutput> Handle(GetMoviePosterFileQuery request, CancellationToken cancellationToken)
    {
        var moviePosterItem = await databaseService.MoviePosters
            .Where(moviePoster => moviePoster.Id == request.MoviePosterId)
            .Select(moviePoster => new
            {
                Id = moviePoster.Id,
                FileName = moviePoster.FileName,
                FileContentType = moviePoster.FileContentType,
                StoredFileId = moviePoster.StoredFileId,
                MovieId = moviePoster.MovieId,
                MovieCreatedBy = moviePoster.Movie.CreatedBy
            })
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new EntityNotFoundException(nameof(MoviePoster), request.MoviePosterId);

        if (moviePosterItem.MovieCreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException($"You cannot get Movie Poster from Movie with Id {moviePosterItem.MovieId} because the Movie is not created by you.");
        }

        var content = await storageService.ReadAsync(moviePosterItem.StoredFileId);

        return new GetMoviePosterFileOutput
        {
            FileName = moviePosterItem.FileName,
            ContentType = moviePosterItem.FileContentType,
            Content = content
        };
    }
}
