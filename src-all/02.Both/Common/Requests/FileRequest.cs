using Microsoft.AspNetCore.Http;

namespace Delta.Polling.Both.Common.Requests;

public abstract record FileRequest
{
    public IFormFile File { get; init; } = default!;
}

public class FileRequestValidator : AbstractValidator<FileRequest>
{
    public FileRequestValidator()
    {
        _ = RuleFor(input => input.File.FileName)
            .MaximumLength(BaseMaxLengthFor.FileName);

        _ = RuleFor(input => input.File.ContentType)
            .MaximumLength(BaseMaxLengthFor.FileContentType);
    }
}
