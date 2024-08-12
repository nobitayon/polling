using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Choices.Commands.AddChoice;
using Delta.Polling.Domain.Choices.Entities;
using Delta.Polling.Services.Storage;

namespace Delta.Polling.Logics.Member.Choices.Commands.AddChoice;

[Authorize(RoleName = RoleNameFor.Member)]
public record AddChoiceCommand : AddChoiceRequest, IRequest<AddChoiceOutput>
{
}

public class AddChoiceCommandValidator : AbstractValidator<AddChoiceCommand>
{
    public AddChoiceCommandValidator()
    {
        Include(new AddChoiceRequestValidator());
    }
}

public class AddChoiceCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService,
    IStorageService storageService)
    : IRequestHandler<AddChoiceCommand, AddChoiceOutput>
{
    public async Task<AddChoiceOutput> Handle(AddChoiceCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var poll = await databaseService.Polls
                        .Where(p => p.Id == request.PollId)
                        .SingleOrDefaultAsync(cancellationToken)
                        ?? throw new Exception($"Poll with Id: {request.PollId} not exist");

        var memberGroup = await databaseService.GroupMembers
                        .Where(gm => gm.GroupId == poll.GroupId)
                        .Select(gm => gm.Username)
                        .ToListAsync(cancellationToken);

        var isInGroup = memberGroup
            .Any(member =>
            {
                return member == currentUserService.Username;
            });

        if (!isInGroup)
        {
            throw new ForbiddenException($"You can't add choice to this poll in this group, because you are not member of this group");
        }

        if (poll.Status != PollStatus.Draft)
        {
            throw new ForbiddenException($"Can't add choice to poll with status that is not draft");
        }

        if (poll.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException("Can't add choice to this draft poll because this poll is not yours");
        }

        var choice = new Choice
        {
            PollId = request.PollId,
            Description = request.Description,
            IsOther = false,
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username
        };

        // We need to loops
        foreach (var mediaItem in request.MediaRequest)
        {
            using var memoryStream = new MemoryStream();
            await mediaItem.File.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;
            var content = memoryStream.ToArray();

            var choiceMediaId = Guid.NewGuid();
            var fileName = $"{choiceMediaId}{Path.GetExtension(mediaItem.File.FileName)}";
            var storedFileId = await storageService.CreateAsync(content, choice.Id.ToString(), fileName);

            var choiceMedia = new ChoiceMedia
            {
                Id = choiceMediaId,
                ChoiceId = choice.Id,
                Created = DateTimeOffset.Now,
                CreatedBy = currentUserService.Username,
                Description = mediaItem.MediaDescription,
                FileName = mediaItem.File.FileName,
                FileContentType = mediaItem.File.ContentType,
                FileSize = mediaItem.File.Length,
                StoredFileId = storedFileId
            };

            _ = await databaseService.ChoiceMedias.AddAsync(choiceMedia, cancellationToken);
        }

        _ = await databaseService.Choices.AddAsync(choice, cancellationToken);

        _ = await databaseService.SaveAsync(cancellationToken);

        return new AddChoiceOutput
        {
            Data = new AddChoiceResult
            {
                ChoiceId = choice.Id
            }
        };
    }
}
