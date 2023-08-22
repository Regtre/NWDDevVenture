using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Controllers;

namespace NWDWebDevelopment.Controllers;

public enum NWDEmailConfigurationPredefine
{
    None,
    Configuration,
    Smtp,
    SmtpSsl,
    SmtpTls,
    Office,
    Gmail,
    Ovh,
}
public enum NWDEmailConfigurationTest
{
    FromWebsiteToEmailTest,
    FromNoReplyToEmailTest,
    EmailTestToWebsite,
    ToWebsite,
    ToWebmaster,
    CaseOfException,
}

public class NWDEmailConfigurationModel
{
    [DisplayName("Library to use")] public NWDEmailConfigurationKind Kind { set; get; } = NWDEmailConfigurationKind.MailKit;

    [Required]
    [DisplayName("Server (SMTP or Exchange)")]
    public string Server { set; get; } = "smtp.office365.com";
    [Required] 
    [DisplayName("Port")] 
    public int Port { set; get; } = 587;
    [Required] 
    [DisplayName("Security")] 
    public NWDSecureSocketOptions Secure { set; get; } = NWDSecureSocketOptions.StartTls;
    [Required]
    [EmailAddress]
    [DisplayName("Email to test (your personal email for example)")]
    public string EmailTester { set; get; } = string.Empty;
    [Required]
    [EmailAddress]
    [DisplayName("User")]
    public string User { set; get; } = string.Empty;
    [Required] 
    [DisplayName("Password")] 
    public string Secret { set; get; } = string.Empty;
    [Required]
    [EmailAddress]
    [DisplayName("Email Website (contact)")]
    public string EmailWebsite { set; get; } = string.Empty;
    [Required] 
    [EmailAddress] 
    [DisplayName("Email No Reply (no_reply)")]
    public string EmailNoReply { set; get; } = string.Empty;
    public bool CopyToWebmaster { set; get; } = true;
    [Required] 
    [EmailAddress] 
    [DisplayName("Email Webmaster (webmaster)")]
    public string EmailWebmaster { set; get; } = string.Empty;
    public string Json { set; get; } = string.Empty;
    public string? Exception { set; get; } = string.Empty;
    public bool Asynchronous { set; get; } = true;
    public NWDEmailConfigurationTest Test  { set; get; }  = NWDEmailConfigurationTest.FromWebsiteToEmailTest;
    public string Template  { set; get; }  = NWDEmailTemplateManager.K_DEFAULT;
    public NWDEmailConfigurationModel()
    {
    }
    public NWDEmailConfigurationModel(NWDEmailConfigurationPredefine sPredefine)
    {
        if (sPredefine != NWDEmailConfigurationPredefine.None)
        {
            Server = "smtp." + NWDEmailConfiguration.KConfig.WebSite.Replace("https://", "").Replace("http://", "").Replace("www.", "");
            User = NWDEmailConfiguration.KConfig.User;
            Secret = NWDEmailConfiguration.KConfig.Password;
            EmailWebsite = NWDEmailConfiguration.KConfig.EmailWebsite;
            EmailTester = NWDEmailConfiguration.KConfig.EmailToReceiptTest;
            CopyToWebmaster = NWDEmailConfiguration.KConfig.CopyToWebmaster;
            EmailWebmaster = NWDEmailConfiguration.KConfig.EmailWebmaster;
            EmailNoReply = NWDEmailConfiguration.KConfig.EmailNoReply;
            switch (sPredefine)
            {
                case NWDEmailConfigurationPredefine.Configuration:
                {
                    Kind = NWDEmailConfigurationKind.SmtpClient;
                    Server = NWDEmailConfiguration.KConfig.Server;
                    Port = NWDEmailConfiguration.KConfig.Port;
                    Secure = NWDEmailConfiguration.KConfig.Secure;
                }
                    break;
                case NWDEmailConfigurationPredefine.Office:
                {
                    Kind = NWDEmailConfigurationKind.MailKit;
                    Server = "smtp.office365.com";
                    Port = 587;
                    Secure = NWDSecureSocketOptions.StartTls;
                }
                    break;
                case NWDEmailConfigurationPredefine.Gmail:
                {
                    Kind = NWDEmailConfigurationKind.SmtpClient;
                    Server = "smtp-relay.gmail.com";
                    Port = 587;
                    Secure = NWDSecureSocketOptions.Auto;
                }
                    break;

                case NWDEmailConfigurationPredefine.Smtp:
                {
                    Kind = NWDEmailConfigurationKind.SmtpClient;
                    Port = 25;
                    Secure = NWDSecureSocketOptions.None;
                }
                    break;

                case NWDEmailConfigurationPredefine.SmtpSsl:
                {
                    Kind = NWDEmailConfigurationKind.SmtpClient;
                    Port = 465;
                    Secure = NWDSecureSocketOptions.Auto;
                }
                    break;
                case NWDEmailConfigurationPredefine.SmtpTls:
                {
                    Kind = NWDEmailConfigurationKind.SmtpClient;
                    Port = 587;
                    Secure = NWDSecureSocketOptions.Auto;
                }
                    break;

                case NWDEmailConfigurationPredefine.Ovh:
                {
                    Kind = NWDEmailConfigurationKind.SmtpClient;
                    Server = "ssl0.ovh.net";
                    Port = 587;
                    Secure = NWDSecureSocketOptions.Auto;
                }
                    break;
            }
        }
    }

    public NWDEmailConfiguration ToConfiguration()
    {
        NWDEmailConfiguration tResult = new NWDEmailConfiguration()
        {
            WebSite = NWDWebRuntimeConfiguration.KConfig.GetDnsHttps(),
            EmailWebsite = EmailWebsite,
            EmailToReceiptTest = EmailTester,
            User = User,
            Password = Secret,
            Server = Server,
            Port = Port,
            Secure = Secure,
            CopyToWebmaster = CopyToWebmaster,
            EmailWebmaster = EmailWebmaster,
            EmailNoReply = EmailNoReply
        };
        return tResult;
    }

    public string ToConfigurationJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(ToConfiguration(), Formatting.Indented);
    }
}

public class NWDEmailConfigurationController : NWDBasicController<NWDEmailConfigurationController>
{
    [NWDAuthorizeAdminOnly()]
    [HttpGet]
    public IActionResult Index(NWDEmailConfigurationPredefine sPredefine = NWDEmailConfigurationPredefine.Configuration)
    {
        NWDEmailConfigurationModel tModel = new NWDEmailConfigurationModel(sPredefine);
        return View(tModel);
    }

    [NWDAuthorizeAdminOnly()]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(NWDEmailConfigurationModel sModel)
    {
        if (ModelState.IsValid)
        {
            NWDEmailManager tEmailManager = new NWDEmailManager();
            NWDEmailResult tResult = new NWDEmailResult();
            string tSubject = "["+sModel.Test.ToString()+"]by " + sModel.Kind.ToString() + " ==" + NWDRandom.RandomStringToken(8).ToUpper()+"==";
            string tMessage = "<h1>Example HTML Message Body</h1> <p>With library " + sModel.Kind.ToString() + "</p>";
            if (sModel.Asynchronous == false)
            {
                switch (sModel.Test)
                {
                    case NWDEmailConfigurationTest.FromWebsiteToEmailTest:
                        tResult = tEmailManager.SendMailNow(sModel.ToConfiguration(), sModel.EmailWebsite, sModel.EmailWebsite, sModel.EmailTester, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.FromNoReplyToEmailTest:
                        tResult = tEmailManager.SendMailNow(sModel.ToConfiguration(), sModel.EmailNoReply, sModel.EmailNoReply, sModel.EmailTester, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.EmailTestToWebsite:
                        tResult = tEmailManager.SendMailNow(sModel.ToConfiguration(), sModel.EmailTester, sModel.EmailTester, sModel.EmailWebsite, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.ToWebsite:
                        tResult = tEmailManager.SendMailNow(sModel.ToConfiguration(), sModel.EmailNoReply, sModel.EmailNoReply, sModel.EmailWebsite, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.ToWebmaster:
                        tResult = tEmailManager.SendMailNow(sModel.ToConfiguration(), sModel.EmailNoReply, sModel.EmailNoReply, sModel.EmailWebmaster, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.CaseOfException:
                        tResult = tEmailManager.SendMailNow(sModel.ToConfiguration(), sModel.EmailNoReply, sModel.EmailNoReply, sModel.EmailWebmaster, tSubject, NWDEmailManager.TransformToMessage(new Exception("Exception description"), HttpContext), NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                }
            }
            else
            {
                switch (sModel.Test)
                {
                    case NWDEmailConfigurationTest.FromWebsiteToEmailTest:
                        tResult = tEmailManager.SendMail(sModel.ToConfiguration(), sModel.EmailWebsite, sModel.EmailWebsite, sModel.EmailTester, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.FromNoReplyToEmailTest:
                        tResult = tEmailManager.SendMail(sModel.ToConfiguration(), sModel.EmailNoReply, sModel.EmailNoReply, sModel.EmailTester, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.EmailTestToWebsite:
                        tResult = tEmailManager.SendMail(sModel.ToConfiguration(), sModel.EmailTester, sModel.EmailTester, sModel.EmailWebsite, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.ToWebsite:
                        tResult = tEmailManager.SendMail(sModel.ToConfiguration(), sModel.EmailNoReply, sModel.EmailNoReply, sModel.EmailWebsite, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.ToWebmaster:
                        tResult = tEmailManager.SendMail(sModel.ToConfiguration(), sModel.EmailNoReply, sModel.EmailNoReply, sModel.EmailWebmaster, tSubject, tMessage, NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                    case NWDEmailConfigurationTest.CaseOfException:
                        tResult = tEmailManager.SendMail(sModel.ToConfiguration(), sModel.EmailNoReply, sModel.EmailNoReply, sModel.EmailWebmaster, tSubject, NWDEmailManager.TransformToMessage(new Exception("Exception description"), HttpContext), NWDEmailTemplateManager.GetCopyByKey(sModel.Template));
                        break;
                }
            }
            if (tResult.Sent == true)
            {
                PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Success, "Email sent", "Email sent successfully by " + sModel.Kind.ToString() + "!", null);
                sModel.Json = sModel.ToConfigurationJson();
            }
            else
            {
                PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Warning, "Email error", "Email not sent!", null);
                sModel.Exception = tResult.Error.ToString();
            }
        }
        else
        {
            PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Warning, "Model error", "Model invalid!", null);
        }
        TryValidateModel(sModel);
        return View(sModel);
    }
}