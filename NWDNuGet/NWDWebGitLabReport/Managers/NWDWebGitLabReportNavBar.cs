using Microsoft.AspNetCore.Http;
using NWDFoundation.Models.Enums;
using NWDFoundation.WebEdition.Enums;
using NWDWebGitLabReport.Configuration;
using NWDWebGitLabReport.Controllers;
using NWDWebGitLabReport.Models;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Controllers;

namespace NWDWebGitLabReport.Managers
{
    public class NWDWebGitLabReportNavBar : INWDNavBar
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
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarApp(HttpContext sHttpContext)
        {
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarDebug(HttpContext sHttpContext)
        {
            if (NWDAuthorizeAdminOnly.ValidFor(sHttpContext) && NWDWebGitLabReportConfiguration.KConfig.Repositories.Count>0)
            {
                NWDNavBarCategory tReturn = new NWDNavBarCategory()
                {
                    Name = "GitLab",
                    IconStyle = "fab fa-git-alt", Elements = new List<NWDNavBarElement>()
                };
                foreach (NWDProjectGitConnection tRepository in NWDWebGitLabReportConfiguration.KConfig.Repositories)
                {
                    tReturn.Elements.Add(new NWDNavBarElement()
                        {
                            Name = tRepository.Name, 
                            ActionName = nameof(NWDGitLabReportController.View), 
                            ControllerName = NWDGitLabReportController.ASP_Controller(), 
                            // BadgeText = tRepository.GitBadge,
                            UrlParameter = "LocalTokenReport="+tRepository.LocalTokenReport,
                        });
                }
                
                return new[] { tReturn };
            }
            return null;
        }
    }
}