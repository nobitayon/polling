using Delta.Polling.FrontEnd.Logics.Emails.Commands.SendTestEmail;

namespace Delta.Polling.WebRP.Pages.Emails;

public class TestModel : PageModelBase
{
    private const char Separator = ',';

    [BindProperty]
    public TestModelInput Input { get; set; } = default!;

    public void OnGet()
    {
        Input = new TestModelInput
        {
            Tos = $"someone1@futorial.net{Separator} someone2@futorial.net",
            Ccs = $"someone.else1@futorial.net{Separator} someone.else2@futorial.net",
            Notes = $"This is a test email on {DateTimeOffset.Now:dd MMMM yyyy HH:mm:sszz}"
        };
    }

    public async Task<IActionResult> OnPost()
    {
        var command = new SendTestEmailCommand
        {
            Tos = Input.Tos.Replace(" ", "").Split(Separator),
            Ccs = Input.Ccs.Replace(" ", "").Split(Separator),
            Notes = Input.Notes
        };

        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
        }

        Notifier.Success("The email has been sent successfully.");

        return Page();
    }
}

public record TestModelInput
{
    public required string Tos { get; set; }
    public required string Ccs { get; set; }
    public required string Notes { get; set; }
}
