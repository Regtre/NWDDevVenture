using System;

namespace NWDEditor.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadEditor
    {
        #region properties

        public ulong RolePublicKey { get; set; }
        
        #endregion
    }
}