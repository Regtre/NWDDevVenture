using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDHub.Models
{

    [Serializable]
    public class NWDProjectGlobalSettings : NWDProjectSubObject
    {
        #region EmailPassword authentification
        [NWDWebPropertyDescription("Email/password", NWDWebEditionStyle.Bool,false, "", "", "", true)]

        public bool AddAccountSignEmailPassword { set; get; } = true;
        [NWDWebPropertyDescription("Email/password => send email", NWDWebEditionStyle.Bool,false, "", "", "", true)]
        public bool AddAccountSignEmailPasswordSendEmail { set; get; } = true;

        #endregion

        #region LoginPassword authentification

        public bool AddAccountSignLoginPassword { set; get; } = true;
        public bool AccountSignInSendEmail { set; get; } = false;
        public bool AccountSignUpSendEmail { set; get; } = false;

        #endregion

        #region LoginEmailPassword authentification

        public bool AddAccountSignLoginPasswordEmail { set; get; } = true;
        public bool AddAccountSignLoginPasswordEmailSendEmail { set; get; } = true;

        #endregion

        #region Google authentification

        public bool AddGoogleSign { set; get; }
        public string GoogleClientId { set; get; } = string.Empty;
        public string GoogleClientSecret { set; get; } = string.Empty;

        #endregion

        #region Facebook authentification

        public bool AddFacebookSign { set; get; }
        public string FacebookClientId { set; get; } = string.Empty;
        public string FacebookClientSecret { set; get; } = string.Empty;

        #endregion

        #region Discord authentification

        public bool AddDiscordSign { set; get; }
        public string DiscordClientId { set; get; } = string.Empty;
        public string DiscordClientSecret { set; get; } = string.Empty;

        #endregion

        #region Apple authentification

        public bool AddAppleSign { set; get; }

        public string AppleServiceId { set; get; } = string.Empty;

        // public string AppleSecretKey { set; get; } = string.Empty;
        public string AppleTeamId { set; get; } = string.Empty;
        public string[] AppleKeyValue { set; get; } = new[] { string.Empty };
        public string AppleKeyId { set; get; } = string.Empty;

        #endregion

        #region Microsoft authentification

        public bool AddMicrosoftSign { set; get; }
        public string MicrosoftClientId { set; get; } = string.Empty;
        public string MicrosoftClientSecret { set; get; } = string.Empty;

        #endregion

        #region Twitter authentification

        public bool AddTwitterSign { set; get; }
        public string TwitterClientId { set; get; } = string.Empty;
        public string TwitterClientSecret { set; get; } = string.Empty;

        #endregion

        #region LinkedIn authentification

        public bool AddLinkedInSign { set; get; }
        public string LinkedInClientId { set; get; } = string.Empty;
        public string LinkedInClientSecret { set; get; } = string.Empty;

        #endregion
        
        #region Social interaction

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

        #endregion
    }
}