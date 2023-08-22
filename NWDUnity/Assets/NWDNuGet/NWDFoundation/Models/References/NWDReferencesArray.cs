#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace NWDFoundation.Models
{
    [Serializable]
    // TODO change for structure
    public class NWDReferencesArray<T> : INWDReferenceArray where T : NWDDatabaseBasicModel
    {
        private ulong[] ReferenceArray { set; get; } = new ulong[] { };

        #region properties

        public const char K_SEPARATOR = ',';
        private string _References = string.Empty;

        public string References
        {
            get
            {
                _References = string.Join(K_SEPARATOR, ReferencesList);
                return _References;
            }
            set
            {
                _References = value;
                ReferencesList = _References.Split(K_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        public List<string> ReferencesList { get; set; } = new List<string>();

        public string SqlValue()
        {
            return string.Join(K_SEPARATOR, ReferencesList);
        }

        
        #endregion

        #region constructors

        public NWDReferencesArray(T? sObject)
        {
            if (sObject != null)
            {
                ReferencesList.Add(sObject.Reference.ToString());
            }
        }

        public NWDReferencesArray(ulong[]? sReferencesArray)
        {
            if (sReferencesArray != null)
            {
                foreach (ulong tRef in sReferencesArray)
                {
                    ReferencesList.Add(tRef.ToString());
                }
            }
        }

        public NWDReferencesArray(T[]? sObjectsArray)
        {
            if (sObjectsArray != null)
            {
                foreach (T tObject in sObjectsArray)
                {
                    ReferencesList.Add(tObject.RowId.ToString());
                }
            }
        }

        public NWDReferencesArray(List<string> sReferencesList)
        {
            ReferencesList = sReferencesList;
        }

        public NWDReferencesArray()
        {
        }

        #endregion

        #region methods

        private List<ulong> GetReferences(string sReferencesString)
        {
            List<ulong> rReferences = new List<ulong>();
            if (string.IsNullOrEmpty(sReferencesString) == false)
            {
                foreach (string tRef in sReferencesString.Split(K_SEPARATOR, StringSplitOptions.RemoveEmptyEntries))
                {
                    rReferences.Add(ulong.Parse(tRef));
                }
            }

            return rReferences;
        }

        public NWDReferencesArray<T> AddValues(ulong[]? sReferencesArray)
        {
            if (sReferencesArray != null)
            {
                foreach (ulong tRef in sReferencesArray)
                {
                    if (!ReferencesList.Contains(tRef.ToString()))
                    {
                        ReferencesList.Add(tRef.ToString());
                    }
                }
            }

            return new NWDReferencesArray<T>(ReferencesList);
        }

        public NWDReferencesArray<T> AddValues(T[]? sObjectsArray)
        {
            if (sObjectsArray != null)
            {
                foreach (T tRef in sObjectsArray)
                {
                    if (!ReferencesList.Contains(tRef.Reference.ToString()))
                    {
                        ReferencesList.Add(tRef.Reference.ToString());
                    }
                }
            }

            return new NWDReferencesArray<T>(ReferencesList);
        }

        public NWDReferencesArray<T> AddValue(T sObject)
        {
            if (!ReferencesList.Contains(sObject.Reference.ToString()))
            {
                ReferencesList.Add(sObject.Reference.ToString());
            }

            return new NWDReferencesArray<T>(ReferencesList);
        }
        public NWDReferencesArray<T> AddValue(string sObjectReference)
        {
            if (!ReferencesList.Contains(sObjectReference.ToString()))
            {
                ReferencesList.Add(sObjectReference.ToString());
            }
            return new NWDReferencesArray<T>(ReferencesList);
        }

        public NWDReferencesArray<T> RemoveValues(ulong[]? sReferencesArray)
        {
            if (sReferencesArray != null)
            {
                foreach (ulong tRef in sReferencesArray)
                {
                    if (ReferencesList.Contains(tRef.ToString()))
                    {
                        ReferencesList.Remove(tRef.ToString());
                    }
                }
            }

            return new NWDReferencesArray<T>(ReferencesList);
        }

        public NWDReferencesArray<T> RemoveValues(T[]? sObjectsArray)
        {
            if (sObjectsArray != null)
            {
                foreach (T tRef in sObjectsArray)
                {
                    if (ReferencesList.Contains(tRef.ToString()))
                    {
                        ReferencesList.Remove(tRef.ToString());
                    }
                }
            }

            return new NWDReferencesArray<T>(ReferencesList);
        }


        // public T[] GetAllObjects()
        // {
        //     List<T> rReturn = new List<T>();
        //     //TODO : NWDConfigRuntime.KConfig. ... get object of Type T with References
        //     return rReturn.ToArray();
        // }

        #endregion
    }
}
#nullable disable