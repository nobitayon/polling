using Delta.Polling.Both.Member.Choices.Queries.GetChoice;

namespace Delta.Polling.Logics.Member.Choices.Queries.GetChoice;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetChoiceQuery : GetChoiceRequest, IRequest<GetChoiceOutput>
{

}

public class GetChoiceQueryValidator : AbstractValidator<GetChoiceQuery>
{
    public GetChoiceQueryValidator()
    {
        Include(new GetChoiceRequestValidator());
    }
}

public class GetChoiceQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetChoiceQuery, GetChoiceOutput>
{
    public async Task<GetChoiceOutput> Handle(GetChoiceQuery request, CancellationToken cancellationToken)
    {
        var choiceItem = await databaseService.Choices
                            .Where(c => c.Id == request.ChoiceId)
                            .Select(c => new ChoiceItem
                            {
                                Id = c.Id,
                                Description = c.Description,
                                IsOther = c.IsOther,
                            }).SingleOrDefaultAsync(cancellationToken)
                            ?? throw new EntityNotFoundException("Choice", request.ChoiceId);

        return new GetChoiceOutput { Data = choiceItem };
    }
}
