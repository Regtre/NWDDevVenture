using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

#nullable enable
namespace NWDFoundation.Tools
{
    [Serializable]
    public abstract class NWDGenericEnum : IComparable
    {
        /// <summary>
        /// The value of the static enum instance
        /// </summary>
        public long Value { set; get; }

        /// <summary>
        /// The name of this enum
        /// </summary>
        public string? Name { set; get; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        public NWDGenericEnum()
        {
            Value = 0;
            Name = null;
        }

        /// <summary>
        /// Convert enum to string with value as string
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return Name?.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert value of enum instance to long
        /// </summary>
        /// <returns></returns>
        public long ToLong()
        {
            return Value;
        }

        /// <summary>
        /// Convert value of enum instance to long
        /// </summary>
        /// <returns></returns>
        public long GetLong()
        {
            return Value;
        }

        public static bool operator ==(NWDGenericEnum? sA, NWDGenericEnum? sB) //TODO fix infinite loop
        {
            if (sA == null && sB == null)
            {
                return true;
            }
            else if (sA == null)
            {
                return false;
            }
            else if (sB == null)
            {
                return false;
            }
            else
            {
                var tTypeMatches = sA.GetType().Equals(sB.GetType());
                if (tTypeMatches)
                {
                    return sA.Value.Equals(sB.Value);
                }
            }

            return false;
        }

        public static bool operator !=(NWDGenericEnum? sA, NWDGenericEnum? sB)
        {
            if (sA == null && sB == null)
            {
                return false;
            }
            else if (sA == null)
            {
                return true;
            }
            else if (sB == null)
            {
                return true;
            }
            else
            {
                var tTypeMatches = sA.GetType().Equals(sB.GetType());
                if (tTypeMatches)
                {
                    return !sA.Value.Equals(sB.Value);
                }
            }

            return false;
        }

        public override bool Equals(object? sObject)
        {
            var tOtherValue = sObject as NWDGenericEnum;
            if (tOtherValue == null)
            {
                return false;
            }
            if (sObject != null)
            {
                var tTypeMatches = GetType().Equals(sObject.GetType());
                var tValueMatches = Value.Equals(tOtherValue.Value);
                return tTypeMatches && tValueMatches;
            }
            return false;
        }

        public int CompareTo(object? sOther)
        {
            if (sOther != null)
            {
                return Value.CompareTo(((NWDGenericEnum)sOther).Value);
            }
            return 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Can create generic class working like dynamical enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class NWDGenericEnumGeneric<T> : NWDGenericEnum where T : NWDGenericEnumGeneric<T>, new()
    {
        private static readonly Dictionary<long, T> KList = new Dictionary<long, T>();

        private static readonly Dictionary<string, T> KStringList = new Dictionary<string, T>();
        //public static K None = Add(0, "None", "None", false);

        public NWDGenericEnumGeneric()
        {
        }

        /// <summary>
        /// Return static instance for this long value
        /// </summary>
        /// <param name="sId"></param>
        /// <returns></returns>
        public static T? GetForValue(long sId)
        {
            T? rReturn = null;
            if (KList.ContainsKey(sId))
            {
                rReturn = KList[sId];
            }

            return rReturn;
        }

        /// <summary>
        /// Return static instance for this string value
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static T? GetForValue(string sName)
        {
            string tCleanedName = NWDStringCleaner.UnixCleaner(sName);
            T? rReturn = null;
            if (KStringList.ContainsKey(tCleanedName))
            {
                rReturn = KStringList[tCleanedName];
            }

            return rReturn;
        }

        /// <summary>
        /// return all keys in of string
        /// </summary>
        /// <returns></returns>
        public static T[] GetAll()
        {
            return KStringList.Values.ToArray();
        }

        /// <summary>
        /// return all keys in of string
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllValues()
        {
            return KStringList.Keys.ToArray();
        }

        public static T? Create(int sId, string sName)
        {
            string tCleanedName = NWDStringCleaner.UnixCleaner(sName);
            T? rReturn = null;
            if (KList.ContainsKey(sId) == false && KStringList.ContainsKey(tCleanedName) == false)
            {
                rReturn = Add(sId, tCleanedName);
            }
            else
            {
                int tId = sId;
                string tName = tCleanedName;
                if (KList.ContainsKey(tId) == true)
                {
                    tId = 10 + tId;
                }

                if (KStringList.ContainsKey(tName) == true)
                {
                    tName = tName + "_" + tId;
                }

                rReturn = Create(tId, tName);
            }

            return rReturn;
        }

        protected static T? Add(int sId, string sName)
        {
            string tCleanedName = NWDStringCleaner.UnixCleaner(sName);
            T? rReturn = null;
            if (KList.ContainsKey(sId) == false && KStringList.ContainsKey(tCleanedName) == false)
            {
                rReturn = Activator.CreateInstance(typeof(T)) as T;
                if (rReturn != null)
                {
                    rReturn.Value = sId;
                    rReturn.Name = tCleanedName;
                    KList.Add(sId, rReturn);
                    KStringList.Add(tCleanedName, rReturn);
                }
            }
            else
            {
                if (KList.ContainsKey(sId))
                {
                    rReturn = KList[sId];
                }
                else if (KStringList.ContainsKey(tCleanedName))
                {
                    rReturn = KStringList[tCleanedName];
                }
            }

            return rReturn;
        }
    }
}
#nullable disable