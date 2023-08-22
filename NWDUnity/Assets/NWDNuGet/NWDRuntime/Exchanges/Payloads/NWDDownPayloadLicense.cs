using System;
using NWDFoundation.Models.Enums;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadLicense
    {
        #region properties

        public bool Success { set; get; } = false;
        public NWDLicenseStatus LicenseValid { set; get; } = NWDLicenseStatus.Unknow;
        public NWDNeedUpdate NeedUpdate { set; get; } = NWDNeedUpdate.Unknow;
        public string Version { set; get; } = string.Empty;

        #endregion
    }
}