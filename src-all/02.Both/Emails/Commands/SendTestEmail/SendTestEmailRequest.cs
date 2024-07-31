namespace Delta.Polling.Both.Emails.Commands.SendTestEmail;

public record SendTestEmailRequest
{
    public required IEnumerable<string> Tos { get; init; }
    public required IEnumerable<string> Ccs { get; init; }
    public required string Notes { get; init; }
}

public class SendTestEmailRequestValidator : AbstractValidator<SendTestEmailRequest>
{
    public SendTestEmailRequestValidator()
    {
        _ = RuleFor(input => input.Tos)
            .Must(x => x.Any());

        _ = RuleFor(input => input.Notes)
            .NotEmpty();
    }
}
