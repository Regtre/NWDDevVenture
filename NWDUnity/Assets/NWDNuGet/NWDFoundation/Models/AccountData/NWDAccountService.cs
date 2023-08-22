using System;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDAccountService : NWDAccountData
    {
        #region properties

        #region Basic References

        public NWDAccountServiceKind ServiceKind { set; get; } = NWDAccountServiceKind.Original;
        public NWDEnvironmentKind EnvironmentKind { set; get; }
        public string Name { set; get; } = string.Empty;
        public long Service { set; get; }

        #endregion

        #region Cascades References

        #region From

        public NWDReference<NWDAccount> OfferByAccount { set; get; } = new NWDReference<NWDAccount>();
        public NWDReference<NWDAccountService> FromAccountService { set; get; } = new NWDReference<NWDAccountService>();

        #endregion

        #region To

        public NWDReference<NWDAccount> ToAccount { set; get; } = new NWDReference<NWDAccount>();
        public NWDReference<NWDAccountService> ToAccountService { set; get; } = new NWDReference<NWDAccountService>();

        #endregion

        #endregion

        #region Account information

        public NWDBootstrapKindOfStyle MessageStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
        public string Message { set; get; } = string.Empty; // use to show special message for this services

        #endregion

        #region Service activation

        public NWDAccountServiceStatus Status { set; get; } = NWDAccountServiceStatus.IsInactive;
        public int Duration { set; get; } = 0;
        public bool UniqueService { set; get; } = false;
        /// <summary>
        /// use to ovveride service by name ... if service with service number and name already exist => override it with this information 
        /// </summary>
        public bool OverrideByName { set; get; } = false;

        #endregion

        #region Service information

        public string Ip { set; get; } = string.Empty;
        public string Cookie { set; get; } = string.Empty;
        public uint OfflineCounterDown { set; get; } = 0;
        public int Start { set; get; } = 0;
        public int End { set; get; } = 0;

        #endregion

        #endregion

        public NWDAccountService()
        {
        }
        
        public bool IsDateActive()
        {
            int tTimestamp = NWDTimestamp.GetTimestampUnix();
            return (Start < tTimestamp && End > tTimestamp);
        }

        public static NWDAccountService CreateSubService(NWDAccountService sService, NWDAccount sToAccount)
        {
            NWDAccountService rService = new NWDAccountService(
                sService.ProjectId,
                sService.EnvironmentKind,
                sToAccount.Reference,
                sService.Service,
                sService.Start, sService.End,
                sService.Message, sService
                    .MessageStyle)
            {
                FromAccountService = new NWDReference<NWDAccountService>(sService.Reference),
                OfferByAccount = new NWDReference<NWDAccount>(sService.Account),
                Name = sService.Name
            };

            return rService; 
        }

        public NWDAccountService(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind, ulong sAccount, long sService, DateTime sStart, DateTime sEnd, string sMessage ="", NWDBootstrapKindOfStyle sMessageStyle = NWDBootstrapKindOfStyle.Primary)
        {
            ProjectId = sProjectId;
            Status = NWDAccountServiceStatus.IsActive;
            Start = NWDTimestamp.ToTimestampUnix(sStart);
            End = NWDTimestamp.ToTimestampUnix(sEnd);
            EnvironmentKind = sEnvironmentKind;
            Account = sAccount;
            Service = sService;
            Message = sMessage;
            MessageStyle = sMessageStyle;
        }
        public NWDAccountService(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind, ulong sAccount, long sService, int sStart, int sEnd, string sMessage ="", NWDBootstrapKindOfStyle sMessageStyle = NWDBootstrapKindOfStyle.Primary)
        {
            ProjectId = sProjectId;
            Status = NWDAccountServiceStatus.IsActive;
            Start = sStart;
            End = sEnd;
            EnvironmentKind = sEnvironmentKind;
            Account = sAccount;
            Service = sService;
            Message = sMessage;
            MessageStyle = sMessageStyle;
        }

        public override bool Equals(object? obj)
        {
            return Reference == (obj as NWDAccountService)?.Reference;
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }
    }
}