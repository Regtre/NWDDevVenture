using NWDFoundation.Models;
using NWDWebRuntime.Tools;
using NWDWebStandard.Models;

namespace NWDWebEditor.Models;

public class NWDTokenVoucher : NWDDatabaseWebBasicModel
{
    public string Token { set; get; } = string.Empty;
    public DateTime StartDate { set; get; } = DateTime.Now;
    public DateTime EndDate { set; get; } = DateTime.Now.AddDays(7);
    public ulong Service { set; get; }
    public bool Used { set; get; } = false;

    public NWDTokenVoucher(NWDVoucher sVoucher)
    {
        StartDate = sVoucher.StartDate;
        EndDate = sVoucher.EndDate;
        Service = sVoucher.Service;
        Token = NWDToolBox.RandomString(32);
    }
    public NWDTokenVoucher()
    { }

    public string GetDurationInDays()
    {
        return EndDate.Subtract(StartDate).Days.ToString();
    }
    

}