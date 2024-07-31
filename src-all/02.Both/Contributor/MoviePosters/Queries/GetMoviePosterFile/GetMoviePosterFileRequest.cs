namespace Delta.Polling.Both.Contributor.MoviePosters.Queries.GetMoviePosterFile;

public record GetMoviePosterFileRequest
{
    public required Guid MoviePosterId { get; init; }
}

public class GetMoviePosterFileRequestValidator : AbstractValidator<GetMoviePosterFileRequest>
{
    public GetMoviePosterFileRequestValidator()
    {
        _ = RuleFor(x => x.MoviePosterId).NotEmpty();
    }
}
