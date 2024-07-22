using System.Text;

namespace Delta.Polling.WebRP.Services.Pager;

public class PagerService
{
    public string GetHtml(string pageName, int totalCount, PaginatedListRequest request)
    {
        var maxPage = (int)Math.Ceiling(totalCount / (decimal)request.PageSize);
        var safeMaxPage = maxPage < 1 ? 1 : maxPage;

        var keyword = string.IsNullOrWhiteSpace(request.SearchText) ? string.Empty : $"&k={request.SearchText}";
        var pageSize = $"&ps={request.PageSize}";

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
            _ = pagerBuilder.Append($"""<a class="page-link" href="{pageName}?p={previousPage}{pageSize}{keyword}">Previous</a>""");
            _ = pagerBuilder.Append("""</li>""");
        }

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
                _ = pagerBuilder.Append($"""<a class="page-link" href="{pageName}?p={i}{pageSize}{keyword}">{i}</a>""");
                _ = pagerBuilder.Append("""</li>""");
            }
        }

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
            _ = pagerBuilder.Append($"""<a class="page-link" href="{pageName}?p={nextPage}{pageSize}{keyword}">Next</a>""");
            _ = pagerBuilder.Append("""</li>""");
        }

        _ = pagerBuilder.Append("""</ul>""");
        _ = pagerBuilder.Append("""</nav>""");

        return pagerBuilder.ToString();
    }
}
