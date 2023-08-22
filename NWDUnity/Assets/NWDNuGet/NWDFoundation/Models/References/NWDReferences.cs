using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDReferences : INWDSubModel, IEnumerable<ulong>
    {
        #region properties
        public List<ulong> References { set; get; }
        #endregion

        #region constructors
        public NWDReferences()
        {
            References = new List<ulong>();
        }

        public NWDReferences(IEnumerable<ulong> sReferences)
        {
            if (sReferences == null)
            {
                References = new List<ulong>();
            }
            else
            {
                References = sReferences.ToList();
            }
        }

        public NWDReferences(IEnumerable<NWDReference> sReferences)
        {
            if (sReferences == null)
            {
                References = new List<ulong>();
            }
            else
            {
                References = sReferences.Select(x => x.Reference).ToList();
            }
        }
        #endregion

        #region methods
        public IEnumerator<ulong> GetEnumerator()
        {
            return References.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return References.GetEnumerator();
        }
        #endregion
    }

    [Serializable]
    public class NWDReferences<T> : NWDReferences, IEnumerable<NWDReference<T>> where T : NWDDatabaseBasicModel
    {
        #region constructors
        public NWDReferences() : base() { }

        public NWDReferences(IEnumerable<ulong> sReferences) : base(sReferences) { }

        public NWDReferences(IEnumerable<NWDReference> sReferences) : base(sReferences) { }

        public NWDReferences(IEnumerable<NWDReference<T>> sReferences) : base(sReferences) { }
        #endregion

        #region methods
        IEnumerator<NWDReference<T>> IEnumerable<NWDReference<T>>.GetEnumerator()
        {
            IEnumerator<ulong> enumerator = References.GetEnumerator();

            while (enumerator.MoveNext())
            {
                yield return new NWDReference<T>(enumerator.Current);
            }
        }
        #endregion
    }
}
