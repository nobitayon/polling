using Microsoft.AspNetCore.Mvc.Razor;

namespace Delta.Polling.WebRP.Common.Extensions;

public static class RazorPageBaseExtensions
{
    public static string GetNavLinkClass(this RazorPageBase pageBase, string menu)
    {
        return pageBase.ViewBag.CurrentMenu == menu ? $"{CssClassFor.NavLink} active" : CssClassFor.NavLink;
    }
}
