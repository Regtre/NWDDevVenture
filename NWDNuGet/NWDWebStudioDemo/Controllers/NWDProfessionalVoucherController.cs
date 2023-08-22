using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;
using NWDWebTreat.Managers;
using Microsoft.Exchange.WebServices.Data;
using MimeKit;
using MimeKit.Text;

namespace NWDWebStudioDemo.Controllers;

[Serializable]
public enum NWDProfessionalTreatmentKind
{
    Home,
    School,
    Hospital,
}

[Serializable]
public class NWDProfessionalTreatmentModel
{
    public const string K_JavaScriptFunction = nameof(NWDContactUsModel) + "_JS";
    [Required] public ulong Account { set; get; }
    public NWDProfessionalTreatmentKind Kind { set; get; }

    [RegularExpression("[0-9]{1,2}")]
    [Required]
    [DisplayName("Offer standard game (in month)")]
    public uint Game { set; get; }

    [RegularExpression("[0-9]{1,2}")]
    [Required]
    [DisplayName("Offer school game (in month)")]
    public uint School { set; get; }

    [RegularExpression("[0-9]{1,2}")]
    [Required]
    [DisplayName("Offer medical game (in month)")]
    public uint Medical { set; get; }

    [DisplayName("Additional message")]
    [MaxLength(256)]
    public string Message { set; get; } = string.Empty;
}

[Serializable]
public class NWDProfessionalVoucherModel : INWDCaptcha
{
    public const string K_JavaScriptFunction = nameof(NWDContactUsModel) + "_JS";
    public ulong Account { set; get; }
    public NWDProfessionalTreatmentKind Kind { set; get; }
    [Required()] [Display(Name = "Name")] public string SenderName { set; get; } = string.Empty;

    [Required()]
    [EmailAddress()]
    [Display(Name = "Email address")]
    public string SenderEmail { set; get; } = string.Empty;

    [Required()]
    [Display(Name = "Message (of up to 512 characters)")]
    [MaxLength(512)]
    public string Message { set; get; } = string.Empty;

    [Required]
    // [RegularExpression("^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$", ErrorMessage = "Please enter phone number international format : (+xx)123456789)")]
    [Phone]
    public string Phone { set; get; } = string.Empty;

    [Required] [MaxLength(64)] public string Country { set; get; } = string.Empty;
    [Required] [MaxLength(128)] public string Address { set; get; } = string.Empty;
    [Required] [MaxLength(64)] public string ZipCode { set; get; } = string.Empty;
    [Required] [MaxLength(64)] public string Job { set; get; } = string.Empty;


    [Required]
    [Display(Name = "Check to consent to store my personal information!")]
    public bool Consent { set; get; } = false;

    [Required()]
    [Display(Name = "Captcha")]

    public string CaptchaValue { set; get; } = string.Empty;
}

public static class NWDWebEmailSendExtensionsProfessionalVoucher
{
    public static void SendEmailFromVoucherByContact(this NWDEmailManager sWebEmailSend, NWDProfessionalVoucherModel sModel, HttpContext sHttpContext)
    {
        try
        {
            Dictionary<string, string> tInfos = new Dictionary<string, string>();
            tInfos.Add("HttpContext.TraceIdentifier", sHttpContext.TraceIdentifier);
            if (sHttpContext.Connection.RemoteIpAddress != null)
            {
                tInfos.Add("HttpContext.Connection.RemoteIpAddress", sHttpContext.Connection.RemoteIpAddress.ToString());
            }

            foreach (string tKey in sHttpContext.Request.Headers.Keys)
            {
                string? tValue = sHttpContext.Request.Headers[tKey];
                if (string.IsNullOrEmpty(tValue) == false)
                {
                    tInfos.Add(tKey, tValue);
                }
            }

            string tInformation = "<table>";
            foreach (KeyValuePair<string, string> tKeyValue in tInfos)
            {
                tInformation = tInformation + "<tr><td><b>" + tKeyValue.Key + "</b></td><td>" + tKeyValue.Value + "</td></tr>";
            }

            tInformation = tInformation + "</table>";
            string tBody =
                "<div>" +
                "<h1>Email From</h1>" +
                "<div>" + sModel.SenderName + " (" + sModel.SenderEmail + ")</div>";
            tBody = tBody +
                    "<h2>Message</h2>" +
                    "<h3>Subject : Special solicitation</h3>";
            tBody = tBody + "<div>" + sModel.Message + "</div>" +
                    "<div>" + NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + NWDProfessionalVoucherController.ASP_Controller() + "/" + nameof(NWDProfessionalVoucherController.AskTreatment) + "/?sModel=" + NWDToolbox.Base64Encode(JsonConvert.SerializeObject(sModel)) + "</div>" +
                    "<h2>Information</h2>" +
                    "<div>" + tInformation + "</div>" +
                    "</div>" +
                    "";
            if (NWDEmailConfiguration.KConfig != null)
            {
                sWebEmailSend.SendToWebsite(sModel.SenderEmail," Special solicitation " + NWDWebRuntimeConfiguration.KConfig.GetDnsHttps(), tBody);
            }
        }
        catch (Exception tException)
        {
            NWDLogger.Exception(tException);
        }
    }
}

public class NWDProfessionalVoucherNavBar : INWDNavBar
{
    static NWDProfessionalVoucherNavBar()
    {
        NWDLogger.Warning(nameof(NWDProfessionalVoucherNavBar) + " is loaded");
    }

    public NWDNavBarMenu[]? AddNavBarMenu(NWDNavBarKind sNavBarKind, HttpContext sHttpContext)
    {
        NWDNavBarMenu tMenu = new NWDNavBarMenu()
        {
            Name = "Professional",
            ActionName = nameof(NWDProfessionalVoucherController.Index),
            ControllerName = nameof(NWDProfessionalVoucherController).Replace("Controller", "")
        };
        return new[] { tMenu };
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
        return null;
    }
}

public class NWDProfessionalVoucherController : NWDBasicController<NWDProfessionalVoucherController>
{
    private static bool RedirectionUrlValidationCallback(string redirectionUrl)
    {
        // The default for the validation callback is to reject the URL.
        bool result = false;
        Uri redirectionUri = new Uri(redirectionUrl);
        // Validate the contents of the redirection URL. In this simple validation
        // callback, the redirection URL is considered valid if it is using HTTPS
        // to encrypt the authentication credentials. 
        if (redirectionUri.Scheme == "https")
        {
            result = true;
        }

        result = true;
        return result;
    }

    public IActionResult Index()
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("jeanfrancois@babaoo.com"));
            email.To.Add(MailboxAddress.Parse("jeanfrancois@babaoo.com"));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Example HTML Message Body</h1>" };
            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
            //smtp.Connect("smtp.babaoo.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("jeanfrancois@babaoo.com", "g/vUXD7CFssKinksA2nd)o");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        catch (Exception sException)
        {
            Console.WriteLine(sException.ToString());
        }

        // NWDWebEmailSend tWebEmailSend = new NWDWebEmailSend();
        // tWebEmailSend.SendRobot("test");
        //
        // ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
        // service.Credentials = new NetworkCredential("jeanfrancois@babaoo.com", "g/vUXD7CFssKinksA2nd)o");
        // service.UseDefaultCredentials = true;
        // service.AutodiscoverUrl("jeanfrancois@babaoo.com", RedirectionUrlValidationCallback);
        // EmailMessage email = new EmailMessage(service);
        // email.From =new EmailAddress("jeanfrancois@babaoo.com");
        // email.ToRecipients.Add(new EmailAddress("jeanfrancois@babaoo.com"));
        // email.Subject = "HelloWorld";
        // email.Body = new MessageBody("This is the first email I've sent by using the EWS Managed API.");
        // email.Send();  
        return View();
    }

    [NWDAuthorizeByAuthentication(true)]
    [HttpGet]
    public IActionResult Ask(NWDProfessionalTreatmentKind sKind)
    {
        NWDProfessionalVoucherModel tModel = new NWDProfessionalVoucherModel();
        tModel.Account = NWDAccountWebManager.GetAccountInContext(HttpContext).Reference;
        tModel.Kind = sKind;
        PageInformation.NewCaptcha(HttpContext);
        return View(tModel);
    }

    [NWDAuthorizeByAuthentication(true)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Ask(NWDProfessionalVoucherModel sModel)
    {
        if (ModelState.IsValid)
        {
            if (NWDCaptcha.TestCaptcha(HttpContext, sModel) == true)
            {
                try
                {
                    NWDEmailManager tEmailer = new NWDEmailManager();
                    tEmailer.SendEmailFromVoucherByContact(sModel, HttpContext);
                    TempData.PutObject<NWDProfessionalVoucherModel>(sModel);
                    return RedirectToAction(nameof(AskSuccess));
                }
                catch (Exception tEx)
                {
                    NWDLogger.Exception(tEx);
                }
            }
            else
            {
                PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Warning, "Captcha error", "Captcha failed!", null);
            }
        }
        else
        {
            PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Warning, "Model error", "Model invalid!", null);
        }

        sModel.CaptchaValue = string.Empty;
        TryValidateModel(sModel);
        PageInformation.NewCaptcha(HttpContext);
        return View(sModel);
    }

    [NWDAuthorizeByAuthentication(true)]
    public IActionResult AskSuccess()
    {
        ViewData[nameof(NWDProfessionalVoucherModel)] = TempData.GetObject<NWDProfessionalVoucherModel>();
        return View();
    }

    [NWDAuthorizeAdminOnly(true)]
    public IActionResult AskTreatment(string sModel)
    {
        NWDProfessionalVoucherModel? tModelObject = JsonConvert.DeserializeObject<NWDProfessionalVoucherModel>(NWDToolbox.Base64Decode(sModel));
        if (tModelObject != null)
        {
            AddViewDataObject(tModelObject);
            NWDProfessionalTreatmentModel tModel = new NWDProfessionalTreatmentModel();
            tModel.Account = tModelObject.Account;
            // vue de la demande
            return View(tModel);
        }
        else
        {
            return RedirectToAction(nameof(Error));
        }
    }

    [NWDAuthorizeAdminOnly(true)]
    public IActionResult AskOperation(NWDProfessionalTreatmentModel sModel)
    {
        NWDLogger.TraceSuccess("je passe par l'operate");

        if (ModelState.IsValid)
        {
            // Traitement de l'opération demande
            NWDLogger.TraceSuccess("le model est valide");
            bool tAccepted = false;
            List<NWDAccountService> tServices = new List<NWDAccountService>();
            if (sModel.Game > 0)
            {
                NWDLogger.TraceSuccess("je dois ajouter " + sModel.Game + " mois au parent");
                NWDAccountService tGameService = new NWDAccountService(NWDWebRuntimeConfiguration.KConfig.MyProjectId, NWDWebRuntimeConfiguration.KConfig.MyEnvironment, sModel.Account, 1000, DateTime.UtcNow, DateTime.UtcNow.AddMonths((int)sModel.Game), "offer by " + NWDWebStandardConfiguration.KConfig.SocietyName);
                tServices.Add(tGameService);
                tGameService.Name = "Standard game demo";
                tGameService.OverrideByName = true;
                tGameService.OfferByAccount = new NWDReference<NWDAccount>(NWDAccountWebManager.GetAccountInContext(HttpContext));
                tGameService.Status = NWDAccountServiceStatus.IsActive;
                tAccepted = true;
            }

            if (sModel.School > 0)
            {
                NWDLogger.TraceSuccess("je dois ajouter " + sModel.School + " mois au prof");
                NWDAccountService tSchoolService = new NWDAccountService(NWDWebRuntimeConfiguration.KConfig.MyProjectId, NWDWebRuntimeConfiguration.KConfig.MyEnvironment, sModel.Account, 2000, DateTime.UtcNow, DateTime.UtcNow.AddMonths((int)sModel.School), "offer by " + NWDWebStandardConfiguration.KConfig.SocietyName);
                tSchoolService.Name = "School game demo";
                tSchoolService.OverrideByName = true;
                tSchoolService.OfferByAccount = new NWDReference<NWDAccount>(NWDAccountWebManager.GetAccountInContext(HttpContext));
                tSchoolService.Status = NWDAccountServiceStatus.IsActive;
                tServices.Add(tSchoolService);
                tAccepted = true;
            }

            if (sModel.Medical > 0)
            {
                NWDLogger.TraceSuccess("je dois ajouter " + sModel.Medical + " mois à l'ortho");
                NWDAccountService tMedicalService = new NWDAccountService(NWDWebRuntimeConfiguration.KConfig.MyProjectId, NWDWebRuntimeConfiguration.KConfig.MyEnvironment, sModel.Account, 3000, DateTime.UtcNow, DateTime.UtcNow.AddMonths((int)sModel.Medical), "offer by " + NWDWebStandardConfiguration.KConfig.SocietyName);
                tMedicalService.Name = "Medical game demo";
                tMedicalService.OverrideByName = true;
                tMedicalService.OfferByAccount = new NWDReference<NWDAccount>(NWDAccountWebManager.GetAccountInContext(HttpContext));
                tMedicalService.Status = NWDAccountServiceStatus.IsActive;
                tServices.Add(tMedicalService);
                tAccepted = true;
            }

            if (tAccepted == true)
            {
                NWDLogger.TraceSuccess("je dois créer les services et les envoyés au TREAT");
                foreach (NWDAccountService tService in tServices)
                {
                    if (NWDTreatRequestManager.AssociateService(tService).Result == false)
                    {
                        PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Warning, "Service error", "Service " + tService.Service + " association failed!", null);
                    }
                }

                NWDLogger.TraceSuccess("je dois envoyer un email au demandeur pour lui annoncer qu'il a beneficier d'une demonstration");
            }
            else
            {
                NWDLogger.TraceSuccess("je dois envoyer un email au demandeur pour lui expliquer qu'il  n'est pas elligible à la demonstration");
            }
        }
        else
        {
            PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Warning, "Model error", "Model invalid!", null);
            return RedirectToAction(nameof(Error));
        }

        return View(sModel);
    }
}