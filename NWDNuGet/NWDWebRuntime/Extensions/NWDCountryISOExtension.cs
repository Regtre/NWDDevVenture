using Microsoft.AspNetCore.Mvc.Rendering;
using NWDFoundation.Localization;

namespace NWDWebRuntime.Extensions;

public static class NWDCountryISOExtension
{
    public static List<SelectListItem> GetCountriesSelectListItem(string? sValue)
    {
        List<SelectListItem> tCountrySelectListItem = new List<SelectListItem>();
        tCountrySelectListItem.Add(new SelectListItem() { Text = "", Value = "" });
        foreach (NWDCountryISO tCountry in NWDCountryISO.List)
        {
            tCountrySelectListItem.Add(new SelectListItem() { Text = tCountry.Name, Value = tCountry.ThreeLetterCode });
        }
        if (string.IsNullOrEmpty(sValue) == false)
        {
            foreach (SelectListItem tItem in tCountrySelectListItem)
            {
                if (tItem.Value == sValue)
                {
                    tItem.Selected = true;
                    break;
                }
            }
        }
        return tCountrySelectListItem;
    }
}