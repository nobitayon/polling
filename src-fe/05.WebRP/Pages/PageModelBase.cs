using AspNetCoreHero.ToastNotification.Abstractions;
using MediatR;

namespace Delta.Polling.WebRP.Pages;

public class PageModelBase : PageModel
{
    private ISender _sender = default!;
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    private INotyfService _notifier = default!;
    protected INotyfService Notifier => _notifier ??= HttpContext.RequestServices.GetRequiredService<INotyfService>();

    public required string PageTitle { get; set; }
    public ProblemDetails? Problem { get; set; }
}
