using ChartJSCore.Models;

namespace NWDWebStandard.Models;

[Serializable]
public class NWDStatisticRow
{
    public string Title { set; get; }= ""; 
    public string Description { set; get; }= ""; 
    public string Footer { set; get; }= ""; 
    public Chart? Graph { set; get; }
}

public class NWDStatisticsGrid
{
    public string Title { set; get; } = ""; 
    public string Description { set; get; } = ""; 
    public string Footer { set; get; } = "";
    public List<NWDStatisticRow> StatisticsList { set; get; } = new List<NWDStatisticRow>();

    public void Add(string sTitle, string sDescription , Chart sChart, string sFooter="")
    {
            StatisticsList.Add(new NWDStatisticRow()
            {
                Title = sTitle,
                Description = sDescription,
                Graph = sChart,
                Footer = sFooter
            });
    }
}