namespace Delta.Polling.Both.Common.Helpers;

public static class PagerHelper
{
    public static int GetSafePageSize(int? pageSize)
    {
        return pageSize is not null ? pageSize.Value : DefaultValueFor.PageSize;
    }

    public static int GetSafePage(int? page)
    {
        return page is null ? DefaultValueFor.CurrentPage : page.Value < 1 ? 1 : page.Value;
    }

    public static int GetSkipAmount(int page, int pageSize)
    {
        var skippedPage = page < 1 ? 0 : page - 1;

        return skippedPage * pageSize;
    }
}
