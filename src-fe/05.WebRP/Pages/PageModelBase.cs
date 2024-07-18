using MediatR;

namespace Delta.Polling.WebRP.Pages;

public class PageModelBase : PageModel
{
    private ISender _sender = default!;

    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
