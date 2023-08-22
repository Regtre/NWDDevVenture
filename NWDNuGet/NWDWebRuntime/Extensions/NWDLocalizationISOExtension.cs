using Microsoft.AspNetCore.Mvc.Rendering;
using NWDFoundation.Localization;

namespace NWDWebRuntime.Extensions;

public static class NWDLocalizationISOExtension
{
    public static List<SelectListItem> GetLanguageSelectListItem(string[]? sValue)
    {
        List<SelectListItem> tCountrySelectListItem = new List<SelectListItem>();
        //tCountrySelectListItem.Add(new SelectListItem() { Text = "", Value = "" });
        foreach (NWDLocalizationISO tLocalization in NWDLocalizationISO.LanguageDico)
        {
            tCountrySelectListItem.Add(new SelectListItem() { Text = tLocalization.Name, Value = tLocalization.ShortCode });
        }

        if (sValue != null)
        {
            foreach (SelectListItem tItem in tCountrySelectListItem)
            {
                if (sValue.Contains(tItem.Value))
                {
                    tItem.Selected = true;
                }
            }
        }

        return tCountrySelectListItem;
    }
}