using Delta.Polling.Both.Member.Choices.Queries.GetChoice;

namespace Delta.Polling.FrontEnd.Logics.Member.Choices.Queries.GetChoice;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetChoiceQuery : GetChoiceRequest, IRequest<ResponseResult<GetChoiceOutput>>
{
}

public class GetChoiceQueryValidator : AbstractValidator<GetChoiceQuery>
{
    public GetChoiceQueryValidator()
    {
        Include(new GetChoiceRequestValidator());
    }
}

public class GetChoiceQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetChoiceQuery, ResponseResult<GetChoiceOutput>>
{
    public async Task<ResponseResult<GetChoiceOutput>> Handle(GetChoiceQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Member/Choices/{request.ChoiceId}", Method.Get);

        return await backEndService.SendRequestAsync<GetChoiceOutput>(restRequest, cancellationToken);
    }
}
