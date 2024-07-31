using Microsoft.AspNetCore.Diagnostics;

namespace Delta.Polling.WebRP.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel(ILogger<ErrorModel> logger)
    : PageModel
{
    public string? ExceptionMessage { get; set; }
    public string? SourcePath { get; set; }

    public void OnGet()
    {
        LoadError();
    }

    public void OnPost()
    {
        LoadError();
    }

    private void LoadError()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature is not null)
        {
            ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            SourcePath = exceptionHandlerPathFeature.Path;

            logger.LogError(exceptionHandlerPathFeature.Error, "Exception Message {ExceptionMessage}", ExceptionMessage);
        }
    }
}
