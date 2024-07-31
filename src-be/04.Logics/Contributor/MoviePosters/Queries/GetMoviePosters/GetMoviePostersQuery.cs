using Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosters;
using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Services.Storage;

namespace Delta.Polling.Logics.Contributor.MoviePosters.Queries.GetMoviePosters;

public record GetMoviePostersQuery : GetMoviePostersRequest, IRequest<GetMoviePostersOutput>
{
}

public class GetMoviePostersQueryValidator : AbstractValidator<GetMoviePostersQuery>
{
    public GetMoviePostersQueryValidator()
    {
        Include(new GetMoviePostersRequestValidator());
    }
}

public class GetMoviePostersQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService,
    IStorageService storageService)
    : IRequestHandler<GetMoviePostersQuery, GetMoviePostersOutput>
{
    public async Task<GetMoviePostersOutput> Handle(GetMoviePostersQuery request, CancellationToken cancellationToken)
    {
        var movieItem = await databaseService.Movies
            .AsNoTracking()
            .Where(movie => movie.Id == request.MovieId)
            .Select(movie => new MovieItem
            {
                Id = movie.Id,
                Title = movie.Title,
                Status = movie.Status,
                Created = movie.Created,
                CreatedBy = movie.CreatedBy,
                Posters = movie.Posters.Select(moviePoster => new MoviePosterItem
                {
                    Id = moviePoster.Id,
                    FileName = moviePoster.FileName,
                    Url = storageService.GetUrl(moviePoster.StoredFileId),
                    Description = moviePoster.Description
                }).ToList()
            })
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Movie), request.MovieId);

        if (movieItem.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException($"You cannot get Movie Posters of Movie with Id {request.MovieId} because the Movie is not created by you.");
        }

        return new GetMoviePostersOutput
        {
            Data = movieItem
        };
    }
}
