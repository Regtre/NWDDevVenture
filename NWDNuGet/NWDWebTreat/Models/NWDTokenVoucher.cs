using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models;
using NWDWebRuntime.Tools;
using NWDWebTreat.Models.Enums;

namespace NWDWebTreat.Models;

public class NWDTokenVoucher : NWDDatabaseWebBasicModel
{
    public string Name { set; get; } = string.Empty;
    public string Token { set; get; } = string.Empty;
    public NWDBootstrapKindOfStyle AlertStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
    public string Alert { set; get; } = string.Empty;
    public NWDVoucherKind Kind { set; get; } = NWDVoucherKind.DateToDate;
    public int Duration { set; get; } = 0;
    public DateTime StartDate { set; get; } = DateTime.UtcNow;
    public DateTime EndDate { set; get; } = DateTime.UtcNow.AddDays(7);
    public long Service { set; get; }
    public bool Used { set; get; } = false;

    public NWDTokenVoucher(NWDVoucher sVoucher)
    {
        StartDate = sVoucher.StartDate;
        EndDate = sVoucher.EndDate;
        Service = sVoucher.Service;
        Kind = sVoucher.Kind;
        Duration = sVoucher.Duration;
        Token = NWDToolBox.RandomString(128);
        Alert = sVoucher.Alert;
        AlertStyle = sVoucher.AlertStyle;
    }
    public NWDTokenVoucher()
    { }

    public bool IsDateActive()
    {
        return (StartDate < DateTime.UtcNow && EndDate > DateTime.UtcNow);
    }
    public string GetDurationInDays()
    {
        return EndDate.Subtract(StartDate).Days.ToString();
    }
    

}