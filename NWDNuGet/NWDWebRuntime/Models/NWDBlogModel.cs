using System.Text;

namespace NWDWebRuntime.Models
{
    [Serializable]
    public enum NWDBlogStyle
    {
        Normal = 0,
        Alert = 1,
    }

    [Serializable]
    public enum NWDBlogCategory
    {
        Default = 0,
        Alert = 1,
    }

    [Serializable]
    public class NWDBlogModel
    {
        public string Reference { set; get; } = string.Empty;
        public DateTime Creation { set; get; }
        public NWDBlogStyle Style { set; get; } = NWDBlogStyle.Normal;
        public NWDBlogCategory Category { set; get; } = NWDBlogCategory.Default;
        public string Title { set; get; } = string.Empty;
        public string Message { set; get; } = string.Empty;

        public NWDBlogModel()
        {
            Creation = DateTime.Now;
        }

        public string ToHtml()
        {
            StringBuilder rReturn = new StringBuilder();

            rReturn.AppendLine("<div class=\"Blog\">");

            rReturn.AppendLine("<div class=\"" + Style.ToString() + "\">");

            rReturn.AppendLine("<div class=\"Title\">");
            rReturn.AppendLine(Title);
            rReturn.AppendLine("</div>");

            rReturn.AppendLine("<div class=\"Category\">");
            rReturn.AppendLine(Category.ToString());
            rReturn.AppendLine("</div>");

            rReturn.AppendLine("<div class=\"Message\">");
            rReturn.AppendLine(Message);
            rReturn.AppendLine("</div>");

            rReturn.AppendLine("</div>");

            rReturn.AppendLine("</div>");

            return rReturn.ToString();
        }

        public void NotNullChecker()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                Title = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Message))
            {
                Message = string.Empty;
            }
        }
    }
}