using Delta.Polling.Both.Member.Polls.Queries.GetRecentStatistics;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetRecentStatistics;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetRecentStatisticsQuery : GetRecentStatisticsRequest, IRequest<ResponseResult<GetRecentStatisticsOutput>>
{
}

public class GetRecentStatisticsQueryValidator : AbstractValidator<GetRecentStatisticsQuery>
{
    public GetRecentStatisticsQueryValidator()
    {
        Include(new GetRecentStatisticsRequestValidator());
    }
}

public class GetRecentStatisticsQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetRecentStatisticsQuery, ResponseResult<GetRecentStatisticsOutput>>
{
    public async Task<ResponseResult<GetRecentStatisticsOutput>> Handle(GetRecentStatisticsQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Member/Polls/recent-statistics", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetRecentStatisticsOutput>(restRequest, cancellationToken);
    }
}
