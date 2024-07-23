namespace Delta.Polling.WebRP.Pages.Shared.ViewComponents;

public class ErrorViewerViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(ErrorResponse errorResponse)
    {
        await Task.CompletedTask;

        return View(errorResponse);
    }
}
