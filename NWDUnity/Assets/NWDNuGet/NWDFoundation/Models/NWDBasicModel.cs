using System;
using NWDFoundation.Exchanges;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDBasicModel : NWDDatabaseBasicModel
    {
        #region properties

        /// <summary>
        /// Project Unique Identity
        /// </summary>
        public ulong ProjectId { set; get; }
        
        public virtual void CopyFrom(NWDBasicModel sOther)
        {
            // Console.WriteLine("CopyFrom NWDBasicModel");
            base.CopyFrom(sOther);
            ProjectId = sOther.ProjectId;
        }
        #endregion
    }
}