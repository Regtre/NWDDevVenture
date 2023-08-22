using System.Text;

namespace NWDWebRuntime.Models
{
    [Serializable]
    public class NWDPartialView
    {
        public int Order { set; get; } = 1;
        public string PartialPath { set; get; } = string.Empty;
        public string PartialHtml { set; get; } = string.Empty;

        public NWDPartialView()
        {
            
        }
        public static NWDPartialView NewNavBarMenuCategory (string sTitle, string sBootstrapIcon, Dictionary<string,string> sNameUrlElements)
        {
            NWDPartialView rReturn = new NWDPartialView();
            StringBuilder tHtml = new StringBuilder();
            tHtml.AppendLine("<div class=\"dropdown-divider\"></div>");
            tHtml.AppendLine("<a class=\"dropdown-item text-secondary disabled\"><span class=\""+sBootstrapIcon+"\"></span> "+sTitle+"</a>");
            foreach (KeyValuePair<string,string> tKeyValue in sNameUrlElements)
            {
                tHtml.AppendLine("<a class=\"dropdown-item\" href=\""+tKeyValue.Value+"\">"+tKeyValue.Key+"</a>");
            }
            rReturn.PartialHtml = tHtml.ToString();
            return rReturn;
        }
    }
}