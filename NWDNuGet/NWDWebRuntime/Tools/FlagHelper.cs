using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWDWebRuntime.Tools
{
    public static class NWDFlagHelper
    {
        public static IEnumerable<SelectListItem> GetMultiSelectList(object sSelected)
        {
            bool tFind = false;
            SelectListItem? tDefault = null;
            int tV = (int)sSelected;
            Type tType = sSelected.GetType();
            List<SelectListItem> rReturn = new List<SelectListItem>();
            string[] tNames = Enum.GetNames(tType);
            foreach (string tName in tNames)
            {
                int tEnum = (int)Enum.Parse(tType, tName);
                SelectListItem tItem = new SelectListItem { Value = tEnum.ToString(), Text = tName };
                //NWDLogger.WriteLine("tEnum = " + Convert.ToString(tEnum, 2) + " tV = " + Convert.ToString(tV, 2));
                if (tEnum != 0)
                {
                    if ((tEnum & tV) != 0)
                    {
                        //NWDLogger.WriteLine(" TRUE tEnum = " + Convert.ToString(tEnum, 2) + " tV = " + Convert.ToString(tV, 2));
                        tItem.Selected = true;
                        tFind = true;
                    }
                }
                if (tEnum == 0)
                {
                    tDefault = tItem;
                }
                rReturn.Add(tItem);
            }
            if (tFind == false)
            {
                if (tDefault != null)
                {
                    tDefault.Selected = true;
                }
            }
            return rReturn;
        }

    }
}