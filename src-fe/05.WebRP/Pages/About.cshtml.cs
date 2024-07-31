namespace Delta.Polling.WebRP.Pages;

public class AboutModel : PageModelBase
{
    public void OnGet()
    {
        Notifier.Information("Hi, there!");
    }
}
