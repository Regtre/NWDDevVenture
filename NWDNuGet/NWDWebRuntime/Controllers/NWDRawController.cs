using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;

namespace NWDWebRuntime.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NWDRawController : Controller
    {
        public static List<Action<HttpContext, NWDRawController>> ListOnActionExecuting = new List<Action<HttpContext, NWDRawController>>();
        public static List<Action<HttpContext, NWDRawController>> ListOnActionExecuted = new List<Action<HttpContext, NWDRawController>>();

        public NWDPageStandard PageInformation = new NWDPageStandard();

        public override void OnActionExecuting(ActionExecutingContext sContext)
        {
            //Console.WriteLine(nameof(NWDRawController) + " " + nameof(OnActionExecuting) + " /" + sContext.RouteData.Values["controller"]?.ToString() +"/" +sContext.RouteData.Values["action"]?.ToString());
            base.OnActionExecuting(sContext);
            string? tController = sContext.RouteData.Values["controller"]?.ToString();
            string? tAction = sContext.RouteData.Values["action"]?.ToString();
            if (tController != null)
            {
                PageInformation.ControllerName = tController;
            }
            if (tAction != null)
            {
                PageInformation.ActionName = tAction;
            }
            RestoreViewDataObject();
            AddViewDataDefault();
            AddViewDataPageStandard(PageInformation);
            if (NWDAccountWebManager.AccountIsConnected(HttpContext))
            {
                NWDWebDataManager.FastSync(HttpContext);
            }
            foreach (Action<HttpContext, NWDRawController> tActionExecuting in ListOnActionExecuting)
            {
                tActionExecuting.Invoke(HttpContext, this);
            }
            if (TempData.HasObject<NWDToastStandard>() == true)
            {
                //Console.WriteLine(nameof(NWDToastStandard) + " is in tempdata");
                NWDToastStandard? tToastStandard = TempData.GetObject<NWDToastStandard>();
                if (tToastStandard != null)
                {
                    //Console.WriteLine(nameof(NWDToastStandard) + " is not null");
                    PageInformation.AddActualToast(tToastStandard);
                }
            }
        }

        public void AddFrequentlyAskedQuestions(NWDFrequentlyAskedQuestionsList? sList = null)
        {
            PageInformation.FrequentlyAskedQuestionsList = NWDFrequentlyAskedQuestionExtension.Find(PageInformation.ControllerName, PageInformation.ActionName);
            if (sList != null)
            {
                PageInformation.FrequentlyAskedQuestionsList.ListOfQuestion.AddRange(sList.ListOfQuestion);
            }
        }
        public override void OnActionExecuted(ActionExecutedContext sContext)
        {
            //Console.WriteLine(nameof(NWDRawController) + " " + nameof(OnActionExecuted) + " /" + sContext.RouteData.Values["controller"]?.ToString() +"/" +sContext.RouteData.Values["action"]?.ToString());
            foreach (Action<HttpContext, NWDRawController> tActionExecuted in ListOnActionExecuted)
            {
                tActionExecuted.Invoke(HttpContext, this);
            }

            base.OnActionExecuted(sContext);
            if (NWDAccountWebManager.AccountIsConnected(HttpContext))
            {
                //NWDLogger.Trace("Sync exit from page" + nameof(OnActionExecuted));
                NWDWebDataManager.SaveDataIfNeeded(HttpContext);
            }
        }

        public void AddViewDataDefault()
        {
            if (ViewData.ContainsKey(typeof(NWDAccountSignOut).Name) == false)
            {
                ViewData[typeof(NWDAccountSignOut).Name] = new NWDAccountSignOut();
            }

            if (ViewData.ContainsKey(typeof(NWDAccountSignUp).Name) == false)
            {
                ViewData[typeof(NWDAccountSignUp).Name] = new NWDAccountSignUp();
            }

            if (ViewData.ContainsKey(typeof(NWDAccountSignIn).Name) == false)
            {
                ViewData[typeof(NWDAccountSignIn).Name] = new NWDAccountSignIn();
            }

            //if (ViewData.ContainsKey(typeof(AccountCookie).Name) == false)
            //{
            //    ViewData[typeof(AccountCookie).Name] = new AccountCookie();
            //}
            foreach (KeyValuePair<string, object?> tT in TempData)
            {
                if (tT.Key.Contains(NWDTempDataExtensions.K_KeyObjects) == false)
                {
                    ViewData[tT.Key] = tT.Value;
                }
            }
        }

        public void AddTempDataObject(INWDTempData sObject)
        {
            TempData.InsertObject(sObject);
        }

        public void AddViewDataObject(Object sObject)
        {
            if (ViewData.ContainsKey(sObject.GetType().Name) == false)
            {
                ViewData[sObject.GetType().Name] = sObject;
            }
        }

        public void AddViewDataPageStandard(NWDPageStandard sPageStandard)
        {
            ViewData[typeof(NWDPageStandard).Name] = sPageStandard;
        }

        public void RestoreViewDataObject()
        {
            RestoreViewDataObject<NWDAccountSignUp>();
            RestoreViewDataObject<NWDAccountSignIn>();
            RestoreViewDataObject<NWDAccountSignOut>();
            RestoreViewDataObject<NWDAccountSignLost>();
            RestoreViewDataObject<NWDAccountSignRescue>();
        }

        public void RestoreViewDataObject<T>() where T : class
        {
            if (TempData.HasObject(typeof(T)) == true)
            {
                T? tObject = TempData.PeekObject<T>();
                ViewData[typeof(T).Name] = tObject;
                try
                {
                    if (tObject != null)
                    {
                        TryUpdateModelAsync<T>(tObject);
                    }
                }
                catch (Exception tEx)
                {
                    NWDLogger.Exception(tEx);
                }
            }
            else
            {
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //Console.WriteLine(nameof(NWDRawController) + "." + nameof(Error) + "();");
            //return View(new StandardErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return View("/Views/Shared/_Error.cshtml");
        }

        public virtual IActionResult AdminOnly()
        {
            //PageInformation.SideBarStyle = NWDSideBarKind.None;
            PageInformation.NavBarStyle = NWDNavBarKind.Home;
            PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Danger, "Forbidden", "You must be admin to use this page!", null);
            return View("/Views/Shared/_AdminOnly.cshtml");
        }

        public virtual IActionResult AdminOnlyWithoutLayout()
        {
            return View("/Views/Shared/_AdminOnlyWithoutLayout.cshtml");
        }

        public virtual IActionResult AccountOnly()
        {
            //PageInformation.SideBarStyle = NWDSideBarKind.None;
            PageInformation.NavBarStyle = NWDNavBarKind.Home;
            PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Danger, "Forbidden", "You must be connected with your account to use this page!", null);
            return View("/Views/Shared/_AccountOnly.cshtml");
        }

        public virtual IActionResult AccountOnlyWithoutLayout()
        {
            return View("/Views/Shared/_AccountOnlyWithoutLayout.cshtml");
        }

        public virtual IActionResult ServiceOnly()
        {
            //PageInformation.SideBarStyle = NWDSideBarKind.None;
            PageInformation.NavBarStyle = NWDNavBarKind.Home;
            PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Danger, "Forbidden", "You must subscript to service to use this page!", null);
            return View("/Views/Shared/_ServiceOnly.cshtml");
        }

        public virtual IActionResult ServiceOnlyWithoutLayout()
        {
            return View("/Views/Shared/_ServiceOnlyWithoutLayout.cshtml");
        }
        //
        // public IActionResult NoRights()
        // {
        //     PageInformation.SideBarStyle = NWDSideBarKind.None;
        //     PageInformation.NavBarStyle = NWDNavBarKind.Home;
        //     PageInformation.AddToastAlert(NWDBootstrapKindOfStyle.Danger, "No rights", "You didn't have rights to consult this page!", new List<string>());
        //     return View("_NoRights.cshtml");
        // }
    }
}