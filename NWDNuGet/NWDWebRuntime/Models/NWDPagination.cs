namespace NWDWebRuntime.Models;

[Serializable]
public class NWDPagination
{
    public List<NWDPageLink> Pages = new List<NWDPageLink>();
    public int TotalPages;
    public int ActualPage;
    public int ResultsTotal;
    public int ResultsPerPage;

    public NWDPagination()
    {
    }

    public NWDPagination(int sTotalPage, int sActivePage)
    {
        Set(sTotalPage, sActivePage);
    }

    public NWDPagination(int sResultsPerPage, int sResultsTotal, int sActivePage)
    {
        Set(sResultsPerPage, sResultsTotal, sActivePage);
    }

    public void Set(int sTotalPage, int sActivePage)
    {
        TotalPages = sTotalPage;
        ActualPage = sActivePage;
        Calculate();
    }

    public void Set(int sResultsPerPage, int sResultsTotal, int sActivePage)
    {
        ResultsPerPage = sResultsPerPage;
        ResultsTotal = sResultsTotal;
        ActualPage = sActivePage;
        TotalPages = (int)MathF.Ceiling(ResultsTotal / ResultsPerPage) + 1;
        Calculate();
    }

    public void Calculate()
    {
        Pages.Clear();
        if (ActualPage > TotalPages)
        {
            ActualPage = TotalPages;
        }
        if (ActualPage < 0)
        {
            ActualPage = 0;
        }
        if (TotalPages == 1)
        {
            Pages.Add(new NWDPageLink("page", false, "page " + ActualPage.ToString(), ActualPage));
        }
        //else if (sTotalPage == 2)
        //{
        //    Pages.Add(new PageLink("page", false, "page " + sActivePage.ToString(), sActivePage));
        //    Pages.Add(new PageLink("page", false, "page " + sActivePage.ToString(), sActivePage));
        //}
        else
        {
            Pages.Add(new NWDPageLink("first", ActualPage != 0, "première", 0));
            Pages.Add(new NWDPageLink("preview", ActualPage != 0, "précédente", ActualPage - 1));
            if (ActualPage > 1)
            {
                Pages.Add(new NWDPageLink("page", true, "page n°" + (ActualPage - 1), ActualPage - 2));
            }
            if (ActualPage > 0)
            {
                Pages.Add(new NWDPageLink("page", true, "page n°" + ActualPage, ActualPage - 1));
            }
            Pages.Add(new NWDPageLink("active", false, "page n°" + (ActualPage + 1), ActualPage));
            if (ActualPage < TotalPages - 1)
            {
                Pages.Add(new NWDPageLink("page", true, "page n°" + (ActualPage + 2), ActualPage + 1));
            }
            if (ActualPage < TotalPages - 2)
            {
                Pages.Add(new NWDPageLink("page", true, "page n°" + (ActualPage + 3), ActualPage + 2));
            }
            Pages.Add(new NWDPageLink("next", ActualPage < TotalPages - 1, "suivante", ActualPage + 1));
            Pages.Add(new NWDPageLink("last", ActualPage < TotalPages - 1, "dernière", TotalPages - 1));
        }
    }
}