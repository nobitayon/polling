namespace Delta.Polling.WebRP.Pages.Shared.ViewComponents;

public class ErrorViewerViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(ProblemDetails problem)
    {
        await Task.CompletedTask;

        return View(problem);
    }
}
