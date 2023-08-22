using System.Globalization;
using System.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using NWDWebRuntime.Configuration;

namespace NWDWebRuntime.Managers
{

    [Serializable]
    public class NWDEmailTemplate
    {
        public const string K_SUBJECT_TAG = "{subject}";
        public const string K_MESSAGE_TAG = "{message}";
        public const string K_WEBSITE_DNS_TAG = "{website_dns}";
        public const string K_WEBSITE_HTTPS_TAG = "{website_https}";
        public string Subject { set; get; } = K_SUBJECT_TAG;
        public string Message { set; get; } = K_MESSAGE_TAG;
        public string Header { set; get; } = string.Empty;
        public string Footer { set; get; } = string.Empty;
        public ResourceManager? PersonalResourceManager = new ResourceManager(typeof(NWDEmailTemplate));

        public NWDEmailTemplate(string sSubject, string sMessage, string sHeader = "", string sFooter ="")
        {
            Subject = sSubject;
            Message = sMessage;
            Header = sHeader;
            Footer = sFooter;
        }

        public NWDEmailTemplate()
        {
        }

        public NWDEmailTemplate(NWDEmailTemplate sEmailTemplate)
        {
            Subject = sEmailTemplate.Subject;
            Message = sEmailTemplate.Message;
            Header = sEmailTemplate.Header;
            Footer = sEmailTemplate.Footer;
        }
        
        public NWDEmailTemplate PrepareFor(HttpContext? sHttpContext, string sSubject, string sMessage)
        {
            CultureInfo? tCulture = null;
            //TODO GET RESOURCES FOR STRING!
            // if (sHttpContext != null)
            // {
            //     IRequestCultureFeature? tRequestCultureFeature = sHttpContext.Request.HttpContext.Features.Get<IRequestCultureFeature>();
            //     tCulture = tRequestCultureFeature?.RequestCulture.Culture;
            // }
            return PrepareFor(tCulture, sSubject, sMessage);
        }
        
        public NWDEmailTemplate PrepareFor(CultureInfo? sCultureInfo, string sSubject, string sMessage)
        {
            NWDEmailTemplate rReturn = new NWDEmailTemplate();
            //TODO GET RESOURCES FOR STRING!
            if (PersonalResourceManager != null && sCultureInfo!=null)
            {
                string? tSubject = PersonalResourceManager.GetString(Subject, sCultureInfo);
                if (string.IsNullOrEmpty(tSubject))
                {
                    tSubject = Subject;
                }
                string? tMessage = PersonalResourceManager.GetString(Message, sCultureInfo);
                if (string.IsNullOrEmpty(tMessage))
                {
                    tMessage = Subject;
                }
                
                string? tHeader = PersonalResourceManager.GetString(Header, sCultureInfo);
                if (string.IsNullOrEmpty(tHeader))
                {
                    tHeader = Header;
                }
                string? tFooter = PersonalResourceManager.GetString(Footer, sCultureInfo);
                if (string.IsNullOrEmpty(tFooter))
                {
                    tFooter = Footer;
                }
                rReturn.Subject = tSubject.Replace(K_SUBJECT_TAG, sSubject);
                rReturn.Message = tHeader+tMessage.Replace(K_MESSAGE_TAG, sMessage)+tFooter;
            }
            else
            {
                rReturn.Subject = Subject.Replace(K_SUBJECT_TAG, sSubject);
                rReturn.Message = Header+Message.Replace(K_MESSAGE_TAG, sMessage)+Footer;
            }
            rReturn.Subject = rReturn.Subject.Replace(K_WEBSITE_HTTPS_TAG, NWDWebRuntimeConfiguration.KConfig.GetDnsHttps()).Replace(K_WEBSITE_DNS_TAG, NWDWebRuntimeConfiguration.KConfig.Dns);
            rReturn.Message = rReturn.Message.Replace(K_WEBSITE_HTTPS_TAG, NWDWebRuntimeConfiguration.KConfig.GetDnsHttps()).Replace(K_WEBSITE_DNS_TAG, NWDWebRuntimeConfiguration.KConfig.Dns);
            return rReturn;
        }

        public NWDEmailTemplate PrepareFor(string sSubject, string sMessage)
        {
            NWDEmailTemplate rReturn = new NWDEmailTemplate();
            rReturn.Subject = Subject.Replace(K_SUBJECT_TAG, sSubject);
            rReturn.Message = Header+Message.Replace(K_MESSAGE_TAG, sMessage)+Footer;
            
            rReturn.Subject = rReturn.Subject.Replace(K_WEBSITE_HTTPS_TAG, NWDWebRuntimeConfiguration.KConfig.GetDnsHttps()).Replace(K_WEBSITE_DNS_TAG, NWDWebRuntimeConfiguration.KConfig.Dns);
            rReturn.Message = rReturn.Message.Replace(K_WEBSITE_HTTPS_TAG, NWDWebRuntimeConfiguration.KConfig.GetDnsHttps()).Replace(K_WEBSITE_DNS_TAG, NWDWebRuntimeConfiguration.KConfig.Dns);
            return rReturn;
        }
    }
}