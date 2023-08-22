using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Controllers;
using NWDWebTreat.Configuration;
using NWDWebTreat.Controllers;

namespace NWDWebTreat.Managers
{
    public class NWDWebTreatNavBar : INWDNavBar
    {
        public NWDNavBarMenu[]? AddNavBarMenu(NWDNavBarKind sNavBarKind, HttpContext sHttpContext)
        {
            return null;
        }
        public NWDNavBarMenu[]? AddNavBarAccount(HttpContext sHttpContext)
        {
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarAdmin(HttpContext sHttpContext)
        {
            return new []{new NWDNavBarCategory()
            {
                Name = "Services Tools",
                IconStyle = "far fa-gem",
                Elements = new List<NWDNavBarElement>()
                {
                    new NWDNavBarElement()
                    {
                        Name = "Voucher",Icon="fas fa-shield-alt", ActionName = nameof(NWDVoucherController.Index), ControllerName = NWDVoucherController.ASP_Controller()
                    },
                }
            }
            };
        }

        public NWDNavBarCategory[]? AddNavBarApp(HttpContext sHttpContext)
        {
            if (NWDAuthorizeByOneOfService.ValidFor(sHttpContext, new NWDGenericServiceEnum[] { NWDGenericServiceEnum.Admin, NWDGenericServiceEnum.Marketing }) == true)
            {
                return new[]
                {
                    new NWDNavBarCategory()
                    {
                        Name = "Services Tools",
                        IconStyle = "far fa-gem",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Marketing voucher", Icon = "fas fa-shield-alt", ActionName = nameof(NWDVoucherController.Index), ControllerName = NWDVoucherController.ASP_Controller()
                            },
                        }
                    }
                };
            }
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarDebug(HttpContext sHttpContext)
        {
            return null;
        }
    }
}