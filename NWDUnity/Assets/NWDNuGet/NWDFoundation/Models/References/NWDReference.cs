using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDReference : INWDSubModel
    {
        #region properties
        public ulong Reference { set; get; }
        #endregion

        #region constructors
        public NWDReference()
        {
            Reference = 0;
        }

        public NWDReference(ulong sReference)
        {
            Reference = sReference;
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }

        static public bool operator ==(NWDReference sRef1, NWDReference sRef2)
        {
            if (sRef1 is null && sRef2 is null)
            {
                return true;
            }

            if (sRef1 is null)
            {
                if (sRef2.Reference == 0)
                {
                    return true;
                }
                return false;
            }

            if (sRef2 is null)
            {
                if (sRef1.Reference == 0)
                {
                    return true;
                }
                return false;
            }

            return sRef1.Reference == sRef2.Reference;
        }

        static public bool operator !=(NWDReference sRef1, NWDReference sRef2)
        {
            return !(sRef1 == sRef2);
        }

        #endregion
    }

    [Serializable]
    public class NWDReference<T> : NWDReference where T : NWDDatabaseBasicModel
    {
        #region constructors
        public NWDReference() : base() { }
        public NWDReference(ulong sReference) : base (sReference) { }
        public NWDReference(T sObject) : base (sObject?.Reference ?? 0) { }
        #endregion
    }
}
