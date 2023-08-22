using System;
using System.Security.Cryptography.X509Certificates;
using NWDFoundation.Tools;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDAccount : NWDBasicModel, INWDAccountRange
    {
        #region properties
        /// <summary>
        /// if ban ... account is ban
        /// </summary>
        public ushort Range { set; get; }
        public bool Ban { set; get; } = false;
        /// <summary>
        /// if trashed ... account will be deleted  at this Timestamp
        /// </summary>
        public int WillBeDeleteAtTimestamp { set; get; }
        /// <summary>
        /// if Migration ... Its data change server ... come back later 
        /// </summary>
        public bool Migration { set; get; } = false;
        /// <summary>
        /// Use to add message for this account (less than 256 char)
        /// </summary>
        public string Message { set; get; } = string.Empty;
        
        public string Payload { set; get; } = string.Empty;

        #endregion
        
        static public ushort ExtractRange(ulong sReference)
        {
            ushort rReturn = ushort.Parse(sReference.ToString().Substring(1, NWDConstants.K_REFERENCE_AREA_GLOBAL.ToString().Length - NWDConstants.K_REFERENCE_AREA_RANGE.ToString().Length));
            return rReturn;
        }

        public override bool Equals(object? obj)
        {
            return Reference == (obj as NWDAccount)?.Reference;
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }
    }
}