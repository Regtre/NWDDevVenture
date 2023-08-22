using Microsoft.AspNetCore.Http;
using NWDFoundation.Models;
using NWDWebRuntime;
using NWDWebRuntime.CallBacks;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebTreat.Models;
using NWDWebTreat.Models.Enums;

namespace NWDWebTreat.Managers;

public class NWDVoucherTokenManager
{
    public static bool ConsumeVoucherToken(HttpContext sHttpContext, string sToken)
    {
        bool rResult = false;
        List<NWDTokenVoucher> tTokenVoucherList = NWDWebDBDataManager.GetBy<NWDTokenVoucher>(
            new Dictionary<string, string>()
            {
                { nameof(NWDTokenVoucher.Token), sToken }
            });
        if (tTokenVoucherList.Count > 0)
        {
            NWDTokenVoucher tTokenVoucher = tTokenVoucherList[0];
           //if (tTokenVoucher.IsDateActive())
            {
                NWDAccount tAccount = NWDAccountWebManager.GetAccountInContext(sHttpContext);
                NWDAccountService? tService = null;
                if (tTokenVoucher is { Used: false })
                {
                    switch (tTokenVoucher.Kind)
                    {
                        case NWDVoucherKind.DateToDate:
                            tService = new NWDAccountService(NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), tAccount.Reference, tTokenVoucher.Service, tTokenVoucher.StartDate, tTokenVoucher.EndDate, tTokenVoucher.Alert, tTokenVoucher.AlertStyle);
                            tService.Name = tTokenVoucher.Name;
                            tService.UniqueService = false;
                            tService.Duration = tTokenVoucher.Duration;
                            tService.Active = true;
                            break;
                        case NWDVoucherKind.AddDuration:
                            DateTime tUseDateTime = DateTime.UtcNow;
                            if (tTokenVoucher.StartDate > tUseDateTime)
                            {
                                tUseDateTime = tTokenVoucher.StartDate;
                            }
                            tService = new NWDAccountService(NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), tAccount.Reference, tTokenVoucher.Service, tUseDateTime, tUseDateTime.AddDays(tTokenVoucher.Duration), tTokenVoucher.Alert, tTokenVoucher.AlertStyle);
                            tService.Name = tTokenVoucher.Name;
                            tService.UniqueService = true;
                            tService.Duration = tTokenVoucher.Duration;
                            tService.Active = true;
                            break;
                    }

                    if (tService != null)
                    {
                        if (NWDTreatRequestManager.AssociateService(tService).Result)
                        {
                            tTokenVoucher.Used = true;
                            NWDWebDBDataManager.SaveData(tTokenVoucher);
                            rResult = true;
                        }
                    }
                }
            }
        }

        return rResult;
    }
}