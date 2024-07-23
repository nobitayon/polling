using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace Delta.Polling.WebRP.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string? ExceptionMessage { get; set; }
    public string? SourcePath { get; set; }

    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature is not null)
        {
            ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            SourcePath = exceptionHandlerPathFeature.Path;
        }
    }

    public void OnPost()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature is not null)
        {
            ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            SourcePath = exceptionHandlerPathFeature.Path;
        }
    }
}
