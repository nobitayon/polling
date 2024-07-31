using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovie;
using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Services.Storage;

namespace Delta.Polling.Logics.Contributor.Movies.Queries.GetMyMovie;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record GetMyMovieQuery : GetMyMovieRequest, IRequest<GetMyMovieOutput>
{
}

public class GetMyMovieQueryValidator : AbstractValidator<GetMyMovieQuery>
{
    public GetMyMovieQueryValidator()
    {
        Include(new GetMyMovieRequestValidator());
    }
}

public class GetMyMovieQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService,
    IStorageService storageService)
    : IRequestHandler<GetMyMovieQuery, GetMyMovieOutput>
{
    public async Task<GetMyMovieOutput> Handle(GetMyMovieQuery request, CancellationToken cancellationToken)
    {
        var movieItem = await databaseService.Movies
            .AsNoTracking()
            .Where(movie => movie.Id == request.MovieId)
            .Select(movie => new MovieItem
            {
                Id = movie.Id,
                Title = movie.Title,
                Storyline = movie.Storyline,
                Budget = movie.Budget,
                Created = movie.Created,
                CreatedBy = movie.CreatedBy,
                Status = movie.Status,
                Approved = movie.Approved,
                ApprovedBy = movie.ApprovedBy,
                Posters = movie.Posters.Select(moviePoster => new MoviePosterItem
                {
                    Id = moviePoster.Id,
                    Url = storageService.GetUrl(moviePoster.StoredFileId),
                    Description = moviePoster.Description
                }).ToList()
            })
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Movie), request.MovieId);

        if (movieItem.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException($"You cannot get Movie with Id {request.MovieId} because the Movie is not created by you.");
        }

        return new GetMyMovieOutput
        {
            Data = movieItem
        };
    }
}
