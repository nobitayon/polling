namespace Delta.Polling.WebRP.Pages.Shared.ViewComponents;

public class ErrorViewerViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(ErrorResponse error)
    {
        await Task.CompletedTask;

        return View(error);
    }
}
