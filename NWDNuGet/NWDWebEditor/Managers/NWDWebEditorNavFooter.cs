using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;

namespace NWDWebEditor.Managers
{
    [Serializable]
    public class NWDWebEditorNavFooter :  INWDNavFooter
    {
        public NWDNavBarCategory[]? AddNavFooterMenu(HttpContext sHttpContext)
        {
            return null;
            NWDNavBarMenu tMenu =
                new NWDNavBarMenu()
                {
                    Name = "Standard",

                    Categories = new List<NWDNavBarCategory>()
                    {
                        new NWDNavBarCategory()
                        {
                            Name = "OUR MISSION",
                            Description = NWDWebStandardConfiguration.KConfig.WebSiteName+" enables front end developers to build custom streamlined user interfaces in a matter of hours, while it gives backend developers all the UI elements they need to develop their web app.And it's rich design can be easily integrated with backends whether your app is based on ruby on rails, laravel, express or any other server side system.",
                            IconStyle = "far fa-comment-alt"
                        },
                        new NWDNavBarCategory()
                        {
                            Name = "COMPANY",
                            IconStyle = "far fa-file-alt", BadgeText = "test",
                            Elements = new List<NWDNavBarElement>()
                            {
                                new NWDNavBarElement()
                                {
                                    Name = "About",Icon="fas fa-info-circle", ActionName = nameof(NWDHomeController.About), ControllerName = NWDHomeController.ASP_Controller()
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Legal notice",Icon="fas fa-landmark", ActionName = nameof(NWDPrivacyController.LegalNotice), ControllerName = NWDPrivacyController.ASP_Controller()
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Terms of Service",Icon="fas fa-cube", ActionName = nameof(NWDPrivacyController.TermsOfService), ControllerName = NWDPrivacyController.ASP_Controller()
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Terms and Conditions",Icon="fas fa-file-signature", ActionName = nameof(NWDPrivacyController.TermsAndConditions), ControllerName = NWDPrivacyController.ASP_Controller()
                                },
                            }
                        },
                        new NWDNavBarCategory()
                        {
                            Name = "PRIVACY",
                            IconStyle = "fas fa-tools",
                            Elements = new List<NWDNavBarElement>()
                            {
                                new NWDNavBarElement()
                                {
                                    Name = "Privacy policy",Icon="fas fa-shield-alt", ActionName = nameof(NWDPrivacyController.PrivacyPolicy), ControllerName = NWDPrivacyController.ASP_Controller()
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "GDPR",Icon="fas fa-database", ActionName = nameof(NWDPrivacyController.GeneralDataProtectionRegulation), ControllerName = NWDPrivacyController.ASP_Controller()
                               },
                                new NWDNavBarElement()
                                {
                                    Name = "Cookies management",Icon="fas fa-cookie", ActionName = nameof(NWDPrivacyController.CookiesManagement), ControllerName = NWDPrivacyController.ASP_Controller()
                                },
                            }
                        },
                        new NWDNavBarCategory()
                        {
                        Name = "OTHER",
                        IconStyle = "fas fa-tools",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Contact-us",Icon="far fa-envelope", ActionName = nameof(NWDHomeController.ContactUs), ControllerName = NWDHomeController.ASP_Controller()
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Statistics",Icon="fas fas fa-cookie", ActionName = nameof(NWDStatisticsConsolidatedController.Statistics), ControllerName = NWDStatisticsConsolidatedController.ASP_Controller()
                            },
                        }
                    }
                    } 
                };
            return tMenu.Categories.ToArray();
        }
    }
}