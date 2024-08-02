using System.Text;

namespace Delta.Polling.WebRP.Services.Pager;

public class PagerService
{
    public string GetHtml(string pageName, int totalCount, PaginatedListRequest request)
    {
        var maxPage = (int)Math.Ceiling(totalCount / (decimal)request.PageSize);
        var safeMaxPage = maxPage < 1 ? 1 : maxPage;

        var keyword = string.IsNullOrWhiteSpace(request.SearchText) && string.IsNullOrWhiteSpace(request.SearchField) ? string.Empty : $"&k={request.SearchText}&kf={request.SearchField}";
        var sortField = request.SortField == null ? string.Empty : $"&sf={request.SortField}";
        var sortOrder = request.SortField == null ? string.Empty : $"&so={request.SortOrder}";
        var pageSize = $"&ps={request.PageSize}";

        Console.WriteLine("Here 4");

        var pagerBuilder = new StringBuilder();
        _ = pagerBuilder.Append("""<nav aria-label="Pager">""");
        _ = pagerBuilder.Append("""<ul class="pagination">""");

        if (request.Page == 1)
        {
            _ = pagerBuilder.Append("""<li class="page-item disabled">""");
            _ = pagerBuilder.Append("""<a class="page-link">Previous</a>""");
            _ = pagerBuilder.Append("""</li>""");
        }
        else
        {
            var previousPage = request.Page - 1;

            _ = pagerBuilder.Append("""<li class="page-item">""");
            _ = pagerBuilder.Append($"""<a class="page-link" href="{pageName}?p={previousPage}{pageSize}{keyword}{sortField}{sortOrder}">Previous</a>""");
            _ = pagerBuilder.Append("""</li>""");
        }

        Console.WriteLine("Here 3");

        for (var i = 1; i <= safeMaxPage; i++)
        {
            if (request.Page == i)
            {
                _ = pagerBuilder.Append("""<li class="page-item active" aria-current="page">""");
                _ = pagerBuilder.Append($"""<a class="page-link" style="cursor: default;">{i}</a>""");
                _ = pagerBuilder.Append("""</li>""");
            }
            else
            {
                _ = pagerBuilder.Append("""<li class="page-item">""");
                _ = pagerBuilder.Append($"""<a class="page-link" href="{pageName}?p={i}{pageSize}{keyword}{sortField}{sortOrder}">{i}</a>""");
                _ = pagerBuilder.Append("""</li>""");
            }
        }

        Console.WriteLine("Here 2");

        if (request.Page == safeMaxPage)
        {
            _ = pagerBuilder.Append("""<li class="page-item disabled">""");
            _ = pagerBuilder.Append("""<a class="page-link">Next</a>""");
            _ = pagerBuilder.Append("""</li>""");
        }
        else
        {
            var nextPage = request.Page + 1;

            _ = pagerBuilder.Append("""<li class="page-item">""");
            _ = pagerBuilder.Append($"""<a class="page-link" href="{pageName}?p={nextPage}{pageSize}{keyword}{sortField}{sortOrder}">Next</a>""");
            _ = pagerBuilder.Append("""</li>""");
        }

        _ = pagerBuilder.Append("""</ul>""");
        _ = pagerBuilder.Append("""</nav>""");

        Console.WriteLine("Here 1");

        return pagerBuilder.ToString();
    }
}
