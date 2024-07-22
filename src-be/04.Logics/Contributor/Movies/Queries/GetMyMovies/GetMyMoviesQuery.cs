using Delta.Polling.Both.Contributor.Movies.Queries.GetMyMovies;

namespace Delta.Polling.Logics.Contributor.Movies.Queries.GetMyMovies;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record GetMyMoviesQuery : GetMyMoviesRequest, IRequest<GetMyMoviesOutput>
{
}

public class GetMyMoviesQueryValidator : AbstractValidator<GetMyMoviesQuery>
{
    public GetMyMoviesQueryValidator()
    {
        Include(new GetMyMoviesRequestValidator());
    }
}

public class GetMyMoviesQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMyMoviesQuery, GetMyMoviesOutput>
{
    public async Task<GetMyMoviesOutput> Handle(GetMyMoviesQuery request, CancellationToken cancellationToken)
    {
        var query = databaseService.Movies
            .AsNoTracking()
            .Where(movie => movie.CreatedBy == currentUserService.Username);

        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            if (string.IsNullOrWhiteSpace(request.SearchField))
            {
                query = query.Where(x => x.Title.Contains(request.SearchText));
            }
            else
            {
                if (request.SearchField == nameof(MovieItem.Title))
                {
                    query = query.Where(x => x.Title.Contains(request.SearchText));
                }
                else if (request.SearchField == nameof(MovieItem.Budget))
                {
                    query = query.Where(x => x.Budget.ToString().Contains(request.SearchText));
                }
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            query = query.OrderBy(movie => movie.Title);
        }
        else
        {
            var sortOrder = request.SortOrder is not null
                ? request.SortOrder.Value
                : SortOrder.Asc;

            if (sortOrder is SortOrder.Asc)
            {
                if (request.SortField == nameof(MovieItem.Title))
                {
                    query = query.OrderBy(movie => movie.Title);
                }
                else if (request.SortField == nameof(MovieItem.Budget))
                {
                    query = query.OrderBy(movie => movie.Budget);
                }
                else if (request.SortField == nameof(MovieItem.Status))
                {
                    query = query.OrderBy(movie => movie.Status);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(MovieItem.Title))
                {
                    query = query.OrderByDescending(movie => movie.Title);
                }
                else if (request.SortField == nameof(MovieItem.Budget))
                {
                    query = query.OrderByDescending(movie => movie.Budget);
                }
                else if (request.SortField == nameof(MovieItem.Status))
                {
                    query = query.OrderByDescending(movie => movie.Status);
                }
            }
            else
            {
                query = query.OrderBy(movie => movie.Title);
            }
        }

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var movies = await query
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(movie => new MovieItem
            {
                Id = movie.Id,
                Title = movie.Title,
                Budget = movie.Budget,
                Status = movie.Status
            })
            .ToListAsync(cancellationToken);

        var output = new GetMyMoviesOutput
        {
            Data = new PaginatedListResponse<MovieItem>
            {
                Items = movies,
                TotalCount = totalCount
            }
        };

        return output;
    }
}
