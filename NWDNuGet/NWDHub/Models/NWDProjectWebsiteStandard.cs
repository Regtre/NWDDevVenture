using NWDFoundation.Configuration.Environments;
using NWDFoundation.WebEdition.Attributes;
using NWDWebStandard.Configuration;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDProjectWebsiteStandardByEnvironment : NWDProjectWebsiteStandard
    {
        [NWDWebPropertyHidden]
        public NWDEnvironmentKind Kind { set; get; } = NWDEnvironmentKind.Dev;
    }

    [Serializable]
    public class NWDProjectWebsiteStandard : NWDProjectDataTrack
    {
        public NWDSignOutMethodInMenu SignOutMethodInMenu { set; get; } = NWDSignOutMethodInMenu.SignOut;
        public bool ShowIdemobiEngine { set; get; } = false;

        public bool ActiveHtmlToPdf { set; get; } = false;

        public string SocietyName { set; get; } = "idéMobi";
        public string SocietyAddress { set; get; } = "43, rue d'Atlanta";
        public string SocietyTown { set; get; } = "Marcq-En-Barœul";
        public string SocietyZipCode { set; get; } = "59700";
        public string SocietyCountry { set; get; } = "FRANCE";
        public string SocietySiret { set; get; } = "44424955100011";
        public string SocietyApe { set; get; } = "7722C";
        public string SocietyRcs { set; get; } = "LILLE";
        public string SocietyTva { set; get; } = "FR93444249551";
        

        public bool AddAccountSignEmailPassword { set; get; } = true;
        public bool AddAccountSignLoginPassword { set; get; } = true;
        public bool AddAccountSignLoginPasswordEmail { set; get; } = true;
        
        
        public bool AddGoogleSign { set; get; }
        public string GoogleClientId { set; get; } = string.Empty;
        public string GoogleClientSecret { set; get; } = string.Empty;
        
        public bool AddFacebookSign { set; get; }
        public string FacebookClientId { set; get; } = string.Empty;
        public string FacebookClientSecret { set; get; } = string.Empty;
        
        public bool AddDiscordSign { set; get; }
        public string DiscordClientId { set; get; } = string.Empty;
        public string DiscordClientSecret { set; get; } = string.Empty;
        public bool AddAppleSign { set; get; }

        public string AppleServiceId { set; get; } = string.Empty;

        // public string AppleSecretKey { set; get; } = string.Empty;
        public string AppleTeamId { set; get; } = string.Empty;
        public string AppleKeyValue { set; get; } = string.Empty;
        public string AppleKeyId { set; get; } = string.Empty;
        
        public bool AddMicrosoftSign { set; get; }
        public string MicrosoftClientId { set; get; } = string.Empty;
        public string MicrosoftClientSecret { set; get; } = string.Empty;
        
        public bool AddTwitterSign { set; get; }
        public string TwitterClientId { set; get; } = string.Empty;
        public string TwitterClientSecret { set; get; } = string.Empty;

        public bool AddLinkedInSign { set; get; }
        public string LinkedInClientId { set; get; } = string.Empty;
        public string LinkedInClientSecret { set; get; } = string.Empty;
        
        
        public bool ShareableByEmail { set; get; } = false;
        public bool ShareableOnFacebook { set; get; } = false;
        public bool ShareableOnGooglePlus { set; get; } = false;
        public bool ShareableOnWhatsApp { set; get; } = false;
        public bool ShareableOnInstagram { set; get; } = false;
        public bool ShareableOnWeibo { set; get; } = false;
        public bool ShareableOnRenren { set; get; } = false;
        public bool ShareableOnBaidu { set; get; } = false;
        public bool ShareableOnReddIt { set; get; } = false;
        public bool ShareableOnTwitter { set; get; } = false;
        public bool ShareableOnPinterest { set; get; } = false;
        public bool ShareableOnLinkedIn { set; get; } = false;
        public bool ShareableOnDiscord { set; get; } = false;
        public bool ShareableOnTwitch { set; get; } = false;
        public bool ShareableOnTumblr { set; get; } = false;
        
        
        public string PageUrlOnFacebook { set; get; } = string.Empty;
        public string PageUrlOnGooglePlus { set; get; } = string.Empty;
        public string PageUrlOnInstagram { set; get; } = string.Empty;
        public string PageUrlOnWeibo { set; get; } = string.Empty;
        public string PageUrlOnReddIt { set; get; } = string.Empty;
        public string PageUrlOnTwitter { set; get; } = string.Empty;
        public string PageUrlOnPinterest { set; get; } = string.Empty;
        public string PageUrlOnLinkedIn { set; get; } = string.Empty;
        public string PageUrlOnDiscord { set; get; } = string.Empty;
        public string PageUrlOnTwitch { set; get; } = string.Empty;
        
        public string WebSiteName { set; get; } = "MyWebSite";
        public string WebSiteShortName { set; get; } = "MyWeb";
        public string WebSiteInitials { set; get; } = "MWS";
        
    }
}