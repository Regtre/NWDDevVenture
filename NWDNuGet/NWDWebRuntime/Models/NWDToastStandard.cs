using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Facades;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDToastStandard: INWDTempData
{
    public NWDWebNotificationLayoutStyle Layout { set; get; } = NWDWebNotificationLayoutStyle.PopUp;
    public NWDBootstrapKindOfStyle Style { set; get; } = NWDBootstrapKindOfStyle.Primary;
    public string Title { set; get; } = string.Empty;
    public string Subtitle { set; get; }= string.Empty;
    public string Message { set; get; }= string.Empty;
    public List<string>? ListOfString { set; get; }

    public string PrimaryUrl { set; get; }= string.Empty;
    public string PrimaryText { set; get; }= string.Empty;

    public string SecondaryUrl { set; get; }= string.Empty;
    public string SecondaryText { set; get; }= string.Empty;

    public bool ContactUs { set; get; } = false;

    public NWDToastStandard()
    {
        
    }
    public static NWDToastStandard CreateToast(NWDWebNotificationLayoutStyle sLayout, NWDBootstrapKindOfStyle sStyle, string sTitle)
    {
        NWDToastStandard rReturn = new NWDToastStandard
        {
            Layout = sLayout,
            Style = sStyle,
            Title = sTitle
        };
        return rReturn;
    }
    public static NWDToastStandard CreateToast(NWDWebNotificationLayoutStyle sLayout, NWDBootstrapKindOfStyle sStyle, string sTitle, string sMessage)
    {
        NWDToastStandard rReturn = new NWDToastStandard
        {
            Layout = sLayout,
            Style = sStyle,
            Title = sTitle,
            Message = sMessage
        };
        return rReturn;
    }
    
    public static NWDToastStandard CreateToast(NWDWebNotificationLayoutStyle sLayout, NWDBootstrapKindOfStyle sStyle, string sTitle, string sMessage, string sSubtitle, bool sContactUs = false, string sPrimaryUrl = "", string sPrimaryText = "", string sSecondaryUrl = "", string sSecondaryText = "")
    {
        NWDToastStandard rReturn = new NWDToastStandard
        {
            Layout = sLayout,
            Style = sStyle,
            Title = sTitle,
            Message = sMessage,
            Subtitle = sSubtitle,
            ContactUs = sContactUs,
            PrimaryUrl = sPrimaryUrl,
            SecondaryUrl = sSecondaryUrl,
            PrimaryText = sPrimaryText,
            SecondaryText = sSecondaryText
        };
        return rReturn;
    }
    
    public static NWDToastStandard CreateToast(NWDWebNotificationLayoutStyle sLayout, NWDBootstrapKindOfStyle sStyle, string sTitle, string sMessage, List<string>? sListOfString, string sSubtitle = "", bool sContactUs = false, string sPrimaryUrl = "", string sPrimaryText = "", string sSecondaryUrl = "", string sSecondaryText = "")
    {
        NWDToastStandard rReturn = new NWDToastStandard
        {
            Layout = sLayout,
            Style = sStyle,
            Title = sTitle,
            Message = sMessage,
            ListOfString = sListOfString,
            Subtitle = sSubtitle,
            ContactUs = sContactUs,
            PrimaryUrl = sPrimaryUrl,
            SecondaryUrl = sSecondaryUrl,
            PrimaryText = sPrimaryText,
            SecondaryText = sSecondaryText
        };
        return rReturn;
    }
}