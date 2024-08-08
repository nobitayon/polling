using Delta.Polling.Both.Member.ChoiceMedias.AddChoiceMedia;
using Delta.Polling.Domain.Choices.Entities;
using Delta.Polling.Services.Storage;

namespace Delta.Polling.Logics.Member.ChoiceMedias.AddChoiceMedia;

[Authorize(RoleName = RoleNameFor.Contributor)]
public record AddChoiceMediaCommand : AddChoiceMediaRequest, IRequest<AddChoiceMediaOutput>
{
}

public class AddChoiceMediaCommandValidator : AbstractValidator<AddChoiceMediaCommand>
{
    public AddChoiceMediaCommandValidator()
    {
        Include(new AddChoiceMediaRequestValidator());
    }
}

public class AddChoiceMediaCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService,
    IStorageService storageService)
    : IRequestHandler<AddChoiceMediaCommand, AddChoiceMediaOutput>
{
    public async Task<AddChoiceMediaOutput> Handle(AddChoiceMediaCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new NotAuthenticatedException();
        }

        var choice = await databaseService.Choices
            .AsNoTracking()
            .Where(x => x.Id == request.ChoiceId)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Choice), request.ChoiceId);

        if (choice.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException($"You cannot add ChoiceMedia to Choice with Id {request.ChoiceId} because the Choice is not created by you.");
        }

        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
        var content = memoryStream.ToArray();

        var choiceMediaId = Guid.NewGuid();
        var fileName = $"{choiceMediaId}{Path.GetExtension(request.File.FileName)}";
        var storedFileId = await storageService.CreateAsync(content, choice.Id.ToString(), fileName);

        var choiceMedia = new ChoiceMedia
        {
            Id = choiceMediaId,
            ChoiceId = choice.Id,
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username,
            Description = request.Description,
            FileName = request.File.FileName,
            FileContentType = request.File.ContentType,
            FileSize = request.File.Length,
            StoredFileId = storedFileId
        };

        _ = await databaseService.ChoiceMedias.AddAsync(choiceMedia, cancellationToken);
        _ = await databaseService.SaveAsync(cancellationToken);

        return new AddChoiceMediaOutput
        {
            Data = new AddChoiceMediaResult
            {
                ChoiceMediaId = choiceMediaId
            }
        };
    }
}
