using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDUnityShared.BasisHelper;
using System;
using UnityEngine;

namespace NWDUnityShared.Scripts
{
    [Serializable]
    public class NWDConnection : ISerializationCallbackReceiver
    {
        [SerializeField] private string SerializedReference;
        public ulong Reference;

        public void OnAfterDeserialize()
        {
            if (!ulong.TryParse(SerializedReference, out Reference))
            {
                Reference = 0;
                NWDLogger.Warning("Counldn't parse value '" + SerializedReference + "' into a reference!\nDefaulted to 0...");
            }
            SerializedReference = null; // Memory optimization, string references will be cleaned by GC.
        }

        public void OnBeforeSerialize()
        {
            SerializedReference = Reference.ToString();
        }
    }

    [Serializable]
    public class NWDConnection<T> : NWDConnection where T : NWDStudioData
    {
        /// <summary>
        /// Implicitly retrieve data from base.
        /// </summary>
        /// <param name="sReference">The reference to convert.</param>
        public static implicit operator T(NWDConnection<T> sReference)
        {
            T rResult = null;
            if (sReference != null)
            {
                rResult = sReference.GetReachableData();
            }
            return rResult;
        }

        /// <summary>
        /// Retrieve Reachable data for account.
        /// </summary>
        /// <returns>The requested data, null if not found.</returns>
        public T GetReachableData()
        {
            return NWDBasisHelper.GetReachableData<T>(Reference);
        }
    }
}

