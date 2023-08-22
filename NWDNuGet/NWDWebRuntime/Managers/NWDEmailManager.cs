using System.Net;
using System.Net.Mail;
using System.Text;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Exchange.WebServices.Data;
using MimeKit;
using MimeKit.Text;
using NWDFoundation.Logger;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Managers;

public class NWDEmailResult
{
    public bool Sent = false;
    public Exception? Error = new Exception("Not executed!");
}

public class NWDEmailManager
{
    public NWDEmailResult SendFromWebsite(string sTo, string sSubject, string sMessage, NWDEmailTemplate? sTemplate = null, HttpContext? sHttpContext = null)
    {
        return _SendMail(NWDEmailConfiguration.KConfig, NWDEmailConfiguration.KConfig.EmailWebsite, NWDEmailConfiguration.KConfig.EmailWebsite, sTo, sSubject, sMessage, sTemplate, sHttpContext);
    }

    public NWDEmailResult SendFromNoReply(string sTo, string sSubject, string sMessage, NWDEmailTemplate? sTemplate = null, HttpContext? sHttpContext = null)
    {
        return _SendMail(NWDEmailConfiguration.KConfig, NWDEmailConfiguration.KConfig.EmailNoReply, NWDEmailConfiguration.KConfig.EmailNoReply, sTo, sSubject, sMessage, sTemplate, sHttpContext);
    }

    public NWDEmailResult SendToWebmaster(string sSubject, string sMessage, NWDEmailTemplate? sTemplate = null, HttpContext? sHttpContext = null)
    {
        return _SendMail(NWDEmailConfiguration.KConfig, NWDEmailConfiguration.KConfig.EmailNoReply, NWDEmailConfiguration.KConfig.EmailNoReply, NWDEmailConfiguration.KConfig.EmailWebmaster, sSubject, sMessage, sTemplate, sHttpContext);
    }

    public NWDEmailResult SendToWebsite(string sReplyTo, string sSubject, string sMessage, NWDEmailTemplate? sTemplate = null, HttpContext? sHttpContext = null)
    {
        return _SendMail(NWDEmailConfiguration.KConfig, NWDEmailConfiguration.KConfig.EmailNoReply, sReplyTo, NWDEmailConfiguration.KConfig.EmailWebsite, sSubject, sMessage, sTemplate, sHttpContext);
    }

    public static string TransformToMessage(Exception sException, HttpContext? sHttpContext = null)
    {
        StringBuilder tMessage = new StringBuilder();
        tMessage.AppendLine("<div><h2>" + sException.Message + "</h2> \n\n\n " + sException.StackTrace + "</div>");
        if (sHttpContext != null)
        {
            string tBodyStr;
            using (StreamReader tReader = new StreamReader(sHttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                tBodyStr = tReader.ReadToEnd();
            }

            tMessage.AppendLine("<div><h3>HttpContext</h3>" + "<div>Request : \n " + sHttpContext.Request.Host + " \n" + sHttpContext.Request.Path + " \n" + "</div><div>Rody : \n " + tBodyStr + "</div>");
        }
        else
        {
            tMessage.AppendLine("<div><h3>HttpContext is null</h3></div>");
        }

        return tMessage.ToString();
    }

    public NWDEmailResult SendMailNow(NWDEmailConfiguration sConfig, string sFrom, string sReplyTo, string sTo, string sSubject, string sMessage, NWDEmailTemplate? sTemplate = null, HttpContext? sHttpContext = null)
    {
        System.Threading.Tasks.Task<NWDEmailResult> tTask = System.Threading.Tasks.Task.Run(() => _SendMailAsync(sConfig, sFrom, sReplyTo, sTo, sSubject, sMessage, sTemplate, sHttpContext));
        return tTask.Result;
        // return _SendMail(sConfig, sFrom, sReplyTo, sTo, sSubject, sMessage);
    }

    public NWDEmailResult SendMail(NWDEmailConfiguration sConfig, string sFrom, string sReplyTo, string sTo, string sSubject, string sMessage, NWDEmailTemplate? sTemplate = null, HttpContext? sHttpContext = null)
    {
        NWDEmailResult rResult = _SendMailAsync(sConfig, sFrom, sReplyTo, sTo, sSubject, sMessage, sTemplate, sHttpContext).Result;
        return rResult;
    }

    public async System.Threading.Tasks.Task<NWDEmailResult> SendMailAsync(NWDEmailConfiguration sConfig, string sFrom, string sReplyTo, string sTo, string sSubject, string sMessage, NWDEmailTemplate? sTemplate = null, HttpContext? sHttpContext = null)
    {
        NWDEmailResult rResult = await _SendMailAsync(sConfig, sFrom, sReplyTo, sTo, sSubject, sMessage, sTemplate, sHttpContext);
        return rResult;
    }

    private NWDEmailResult _SendMail(NWDEmailConfiguration? sConfig, string sFrom, string sReplyTo, string sTo, string sSubjectOrigin, string sMessageOrigin, NWDEmailTemplate? sTemplateBase, HttpContext? sHttpContext)
    {
        NWDLogger.Information(nameof(_SendMailAsync));
        NWDEmailResult rResult = new NWDEmailResult();

        if (sConfig != null)
        {
        NWDEmailTemplate tTemplate;
        if (sTemplateBase == null)
        {
            tTemplate = NWDEmailTemplateManager.GetCopyByKey(NWDEmailTemplateManager.K_DEFAULT);
        }
        else
        {
            tTemplate = new NWDEmailTemplate(sTemplateBase);
        }

        if (sHttpContext != null)
        {
            tTemplate = tTemplate.PrepareFor(sHttpContext, sSubjectOrigin, sMessageOrigin);
        }
        else
        {
            tTemplate = tTemplate.PrepareFor(sSubjectOrigin, sMessageOrigin);
        }
            switch (sConfig.Kind)
            {
                case NWDEmailConfigurationKind.MailKit:
                {
                    try
                    {
                        var tEmail = new MimeMessage();
                        tEmail.From.Add(MailboxAddress.Parse(sFrom));
                        if (string.IsNullOrEmpty(sReplyTo) == false)
                        {
                            tEmail.ReplyTo.Add(MailboxAddress.Parse(sReplyTo));
                        }

                        tEmail.To.Add(MailboxAddress.Parse(sTo));
                        tEmail.Subject = tTemplate.Subject;
                        tEmail.Body = new TextPart(TextFormat.Html) { Text = tTemplate.Message };
                        using var tSmtp = new MailKit.Net.Smtp.SmtpClient();
                        SecureSocketOptions tSecure = SecureSocketOptions.Auto;
                        switch (sConfig.Secure)
                        {
                            case NWDSecureSocketOptions.Auto:
                                tSecure = SecureSocketOptions.Auto;
                                break;
                            case NWDSecureSocketOptions.None:
                                tSecure = SecureSocketOptions.None;
                                break;
                            case NWDSecureSocketOptions.StartTls:
                                tSecure = SecureSocketOptions.StartTls;
                                break;
                            case NWDSecureSocketOptions.SslOnConnect:
                                tSecure = SecureSocketOptions.SslOnConnect;
                                break;
                            case NWDSecureSocketOptions.StartTlsWhenAvailable:
                                tSecure = SecureSocketOptions.StartTlsWhenAvailable;
                                break;
                        }

                        tSmtp.Connect(sConfig.Server, sConfig.Port, tSecure);
                        tSmtp.Authenticate(sConfig.User, sConfig.Password);
                        tSmtp.Send(tEmail);
                        tSmtp.Disconnect(true);
                        rResult.Sent = true;
                    }
                    catch (Exception tException)
                    {
                        rResult.Sent = false;
                        rResult.Error = tException;
                        Console.WriteLine(tException.ToString());
                    }
                }
                    break;

                case NWDEmailConfigurationKind.SmtpClient:
                {
                    try
                    {
                        System.Net.Mail.SmtpClient tClient = new System.Net.Mail.SmtpClient(sConfig.Server, sConfig.Port);
                        if (sConfig.Secure == NWDSecureSocketOptions.None)
                        {
                            tClient.EnableSsl = false;
                        }
                        else
                        {
                            tClient.EnableSsl = true;
                        }

                        tClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        tClient.UseDefaultCredentials = false;
                        tClient.Credentials = new NetworkCredential(sConfig.User, sConfig.Password);
                        MailMessage tMessage = new MailMessage(sFrom, sTo, tTemplate.Subject, tTemplate.Message);
                        if (string.IsNullOrEmpty(sReplyTo) == false)
                        {
                            tMessage.ReplyToList.Add(new MailAddress(sReplyTo));
                        }

                        tMessage.IsBodyHtml = true;
                        tClient.Send(tMessage);
                        rResult.Sent = true;
                    }
                    catch (Exception tException)
                    {
                        rResult.Sent = false;
                        rResult.Error = tException;
                        Console.WriteLine(tException.ToString());
                    }
                }
                    break;
                case NWDEmailConfigurationKind.Exchange:
                {
                    try
                    {
                        ExchangeService tService = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
                        tService.Credentials = new NetworkCredential(sConfig.User, sConfig.Password);
                        tService.UseDefaultCredentials = true;
                        tService.AutodiscoverUrl(sConfig.User, RedirectionUrlValidationCallback);
                        EmailMessage tEmail = new EmailMessage(tService);
                        tEmail.From = new EmailAddress(sFrom);
                        if (string.IsNullOrEmpty(sReplyTo) == false)
                        {
                            tEmail.ReplyTo.Add(new EmailAddress(sReplyTo));
                        }

                        tEmail.ToRecipients.Add(new EmailAddress(sTo));
                        tEmail.Subject = tTemplate.Subject;
                        tEmail.Body = new MessageBody(tTemplate.Message);
                        tEmail.Send();
                        rResult.Sent = true;
                    }
                    catch (Exception tException)
                    {
                        rResult.Sent = false;
                        rResult.Error = tException;
                        Console.WriteLine(tException.ToString());
                    }
                }
                    break;
            }

            if (sConfig.CopyToWebmaster == true && sTo != sConfig.EmailWebmaster)
            {
                _SendMail(sConfig, sConfig.EmailNoReply, sConfig.EmailNoReply, sConfig.EmailWebmaster, "[Carbon " + sTo + "] " + sSubjectOrigin, sMessageOrigin, sTemplateBase, sHttpContext);
            }
        }
        else
        {
            NWDLogger.Warning(nameof(NWDEmailConfiguration) + " " + " is not instantiated!");
        }

        return rResult;
    }

    private static bool RedirectionUrlValidationCallback(string sRedirectionUrl)
    {
        // The default for the validation callback is to reject the URL.
        bool result = false;
        Uri redirectionUri = new Uri(sRedirectionUrl);
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

    private async System.Threading.Tasks.Task<NWDEmailResult> _SendMailAsync(NWDEmailConfiguration? sConfig, string sFrom, string sReplyTo, string sTo, string sSubjectOrigin, string sMessageOrigin, NWDEmailTemplate? sTemplateBase, HttpContext? sHttpContext)
    {
        NWDLogger.Information(nameof(_SendMailAsync));
        NWDEmailResult rResult = new NWDEmailResult();
        if (sConfig != null)
        {
        NWDEmailTemplate tTemplate;
        if (sTemplateBase == null)
        {
            tTemplate = NWDEmailTemplateManager.GetCopyByKey(NWDEmailTemplateManager.K_DEFAULT);
        }
        else
        {
            tTemplate = new NWDEmailTemplate(sTemplateBase);
        }

        if (sHttpContext != null)
        {
            tTemplate = tTemplate.PrepareFor(sHttpContext, sSubjectOrigin, sMessageOrigin);
        }
        else
        {
            tTemplate = tTemplate.PrepareFor(sSubjectOrigin, sMessageOrigin);
        }
            switch (sConfig.Kind)
            {
                case NWDEmailConfigurationKind.MailKit:
                {
                    try
                    {
                        var tEmail = new MimeMessage();
                        tEmail.From.Add(MailboxAddress.Parse(sFrom));
                        if (string.IsNullOrEmpty(sReplyTo) == false)
                        {
                            tEmail.ReplyTo.Add(MailboxAddress.Parse(sReplyTo));
                        }

                        tEmail.To.Add(MailboxAddress.Parse(sTo));
                        tEmail.Subject = tTemplate.Subject;
                        tEmail.Body = new TextPart(TextFormat.Html) { Text = tTemplate.Message };
                        using var tSmtp = new MailKit.Net.Smtp.SmtpClient();
                        SecureSocketOptions tSecure = SecureSocketOptions.Auto;
                        switch (sConfig.Secure)
                        {
                            case NWDSecureSocketOptions.Auto:
                                tSecure = SecureSocketOptions.Auto;
                                break;
                            case NWDSecureSocketOptions.None:
                                tSecure = SecureSocketOptions.None;
                                break;
                            case NWDSecureSocketOptions.StartTls:
                                tSecure = SecureSocketOptions.StartTls;
                                break;
                            case NWDSecureSocketOptions.SslOnConnect:
                                tSecure = SecureSocketOptions.SslOnConnect;
                                break;
                            case NWDSecureSocketOptions.StartTlsWhenAvailable:
                                tSecure = SecureSocketOptions.StartTlsWhenAvailable;
                                break;
                        }

                        tSmtp.Connect(sConfig.Server, sConfig.Port, tSecure);
                        tSmtp.Authenticate(sConfig.User, sConfig.Password);
                        await tSmtp.SendAsync(tEmail);
                        tSmtp.Disconnect(true);
                        rResult.Sent = true;
                    }
                    catch (Exception tException)
                    {
                        rResult.Sent = false;
                        rResult.Error = tException;
                        Console.WriteLine(tException.ToString());
                    }
                }
                    break;

                case NWDEmailConfigurationKind.SmtpClient:
                {
                    try
                    {
                        System.Net.Mail.SmtpClient tClient = new System.Net.Mail.SmtpClient(sConfig.Server, sConfig.Port);
                        if (sConfig.Secure == NWDSecureSocketOptions.None)
                        {
                            tClient.EnableSsl = false;
                        }
                        else
                        {
                            tClient.EnableSsl = true;
                        }

                        tClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        tClient.UseDefaultCredentials = false;
                        tClient.Credentials = new NetworkCredential(sConfig.User, sConfig.Password);
                        MailMessage tMessage = new MailMessage(sFrom, sTo, tTemplate.Subject, tTemplate.Message);
                        if (string.IsNullOrEmpty(sReplyTo) == false)
                        {
                            tMessage.ReplyToList.Add(new MailAddress(sReplyTo));
                        }

                        tMessage.IsBodyHtml = true;
                        await tClient.SendMailAsync(tMessage);
                        rResult.Sent = true;
                    }
                    catch (Exception tException)
                    {
                        rResult.Sent = false;
                        rResult.Error = tException;
                        Console.WriteLine(tException.ToString());
                    }
                }
                    break;
                case NWDEmailConfigurationKind.Exchange:
                {
                    try
                    {
                        ExchangeService tService = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
                        tService.Credentials = new NetworkCredential(sConfig.User, sConfig.Password);
                        tService.UseDefaultCredentials = true;
                        tService.AutodiscoverUrl(sConfig.User, RedirectionUrlValidationCallback);
                        EmailMessage tEmail = new EmailMessage(tService);
                        tEmail.From = new EmailAddress(sFrom);
                        if (string.IsNullOrEmpty(sReplyTo) == false)
                        {
                            tEmail.ReplyTo.Add(new EmailAddress(sReplyTo));
                        }

                        tEmail.ToRecipients.Add(new EmailAddress(sTo));
                        tEmail.Subject = tTemplate.Subject;
                        tEmail.Body = new MessageBody(tTemplate.Message);
                        await tEmail.Send();
                        rResult.Sent = true;
                    }
                    catch (Exception tException)
                    {
                        rResult.Sent = false;
                        rResult.Error = tException;
                        Console.WriteLine(tException.ToString());
                    }
                }
                    break;
            }

            if (sConfig.CopyToWebmaster == true && sTo != sConfig.EmailWebmaster)
            {
                await _SendMailAsync(sConfig, sConfig.EmailNoReply, sConfig.EmailNoReply, sConfig.EmailWebmaster, "[Carbon " + sTo + "] " + sSubjectOrigin, sMessageOrigin, sTemplateBase, sHttpContext);
            }
        }
        else
        {
            NWDLogger.Warning(nameof(NWDEmailConfiguration) + " " + " is not instantiated!");
        }

        return rResult;
    }
}