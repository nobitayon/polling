namespace Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosters;

public record GetMoviePostersRequest
{
    public required Guid MovieId { get; set; }
}

public class GetMoviePostersRequestValidator : AbstractValidator<GetMoviePostersRequest>
{
    public GetMoviePostersRequestValidator()
    {
        _ = RuleFor(x => x.MovieId)
            .NotEmpty();
    }
}
