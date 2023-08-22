using Microsoft.AspNetCore.Mvc;
using NWDWebEditor.Managers;
using NWDWebEditor.Models;
using NWDWebRuntime.Managers;

namespace NWDWebEditor.Controllers
{

    public class NWDAdminController : NWDEditorController<NWDAdminController>
    {
        [NWDAuthorizeAdminOnly()]
        public IActionResult Index()
        { 
            return View();
        }


        [HttpGet]
        [HttpPost]
        [NWDAuthorizeAdminOnly()]
        public IActionResult Tools(NWDReportModel sReportModel)
        {
                ViewData.Add(nameof(NWDFindAccountByEmailModel), new NWDFindAccountByEmailModel());
                return View(nameof(Tools));
        }

        [HttpPost]
        [NWDAuthorizeAdminOnly()]
        public IActionResult FindAccountByEmail(NWDFindAccountByEmailModel sFindAccountByEmailModel)
        {
                sFindAccountByEmailModel.Reference = NWDAccountSignWebManager.GetAccountByEmail(sFindAccountByEmailModel.Email);
                ViewData.Add(nameof(NWDFindAccountByEmailModel), sFindAccountByEmailModel);
                return View(nameof(Tools));
        }

        [HttpGet]
        [HttpPost]
        [NWDAuthorizeAdminOnly()]
        public IActionResult AddServiceModel(NWDAddServiceModel sAddServiceModel)
        {
                if (ModelState.IsValid)
                {
                    // if (AccountModel.TestIfAccountExists(sAddServiceModel.Reference))
                    // {
                    //     AccountService tAccountService = null;
                    //     foreach (AccountService tservice in AccountService.GetServiceByAccount(sAddServiceModel.Reference))
                    //     {
                    //         if (tservice.ServiceName == sAddServiceModel.Service)
                    //         {
                    //             tAccountService = tservice;
                    //             break;
                    //         }
                    //     }
                    //     if (tAccountService == null)
                    //     {
                    //         tAccountService = new AccountService();
                    //         tAccountService.Account = sAddServiceModel.Reference;
                    //         tAccountService.Reference = ToolBox.NewValidReference(typeof(AccountService), "SRVF");
                    //         tAccountService.ServiceName = sAddServiceModel.Service;
                    //         tAccountService.End = ToolBox.GetTimestampUnix() + sAddServiceModel.Days * 24 * 60 * 60;
                    //         tAccountService.Reccord();
                    //
                    //     }
                    //     else
                    //     {
                    //         tAccountService.End = Math.Max(tAccountService.End, ToolBox.GetTimestampUnix()) + sAddServiceModel.Days * 24 * 60 * 60;
                    //         tAccountService.Update();
                    //     }
                    // }
                }
                return View(nameof(Tools));
        }

    }
}
