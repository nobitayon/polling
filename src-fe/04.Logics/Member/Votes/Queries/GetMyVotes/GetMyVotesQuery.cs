using Delta.Polling.Both.Member.Votes.Queries.GetMyVotes;

namespace Delta.Polling.FrontEnd.Logics.Member.Votes.Queries.GetMyVotes;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetMyVotesQuery : GetMyVotesRequest, IRequest<ResponseResult<GetMyVotesOutput>>
{
}

public class GetMyVotesQueryValidator : AbstractValidator<ResponseResult<GetMyVotesQuery>>
{
    public GetMyVotesQueryValidator()
    {
        Include(new GetMyVotesQueryValidator());
    }
}

public class GetMyVotesQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetMyVotesQuery, ResponseResult<GetMyVotesOutput>>
{
    public async Task<ResponseResult<GetMyVotesOutput>> Handle(GetMyVotesQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"member/votes", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetMyVotesOutput>(restRequest, cancellationToken);
    }
}
