using System;
using System.ComponentModel;
using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
#if NETCOREAPP
using System.ComponentModel.DataAnnotations;
#endif

namespace NWDWebRuntime.Models
{
    public class NWDAccountInfo : NWDPlayerData
    {
        [NWDWebNoHtml()]
#if NETCOREAPP
        [StringLength(128)]
#endif
        [DisplayName("Firstname")]
        public string Firstname { set; get; }
        [NWDWebNoHtml()]
#if NETCOREAPP
        [StringLength(128)]
#endif
        [DisplayName("Lastname")]
        public string Lastname { set; get; }
        [NWDWebNoHtml()]
#if NETCOREAPP
        [StringLength(24)]
#endif
        [NWDWebUnixText]
        [DisplayName("Nickname")]
        public string Nickname { set; get; } = string.Empty;
#if NETCOREAPP
        [EmailAddress]
#endif
        [DisplayName("Gravatar account")]
        public string GravatarAccount { set; get; } = string.Empty;
        [DisplayName("Gravatar unique Id")]
        public string GravatarHash { set; get; } = string.Empty;

        public NWDAccountInfo()
        {
        }

        public NWDAccountInfo(NWDAccountInfo sAccountInfo)
        {
            // Console.WriteLine("new NWDAccountInfo");
            base.CopyFrom(sAccountInfo);
            Firstname = sAccountInfo.Firstname;
            Lastname = sAccountInfo.Lastname;
            Nickname = sAccountInfo.Nickname;
            GravatarAccount = sAccountInfo.GravatarAccount;
            GravatarHash = sAccountInfo.GravatarHash;
        }
    }
}