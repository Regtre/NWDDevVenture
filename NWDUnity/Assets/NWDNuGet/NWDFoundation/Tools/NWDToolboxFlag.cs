using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace NWDFoundation.Tools
{
    public partial class NWDToolboxEnum
    {
        public static K StringConvertToFlag<K>(string sValue) where K : Enum
        {
            if (string.IsNullOrEmpty(sValue) == false)
            {
                return (K)Enum.Parse(typeof(K), sValue);
            }
            else
            {
                return (K)Activator.CreateInstance(typeof(K));
            }
        }

        public static string FlagConvertToString<K>(K sValue) where K : Enum
        {
            return sValue.ToString();
        }

        public static K Int32ConvertToFlag<K>(Int32 sValue) where K : Enum
        {
            return (K)Enum.ToObject(typeof(K), sValue);
        }

        public static Int32 FlagConvertToInt32<K>(K sValue) where K : Enum
        {
            return (Int32)Convert.ToInt32(sValue);
        }

        public static K Int64ConvertToFlag<K>(Int64 sValue) where K : Enum
        {
            return (K)Enum.ToObject(typeof(K), sValue);
        }

        public static Int64 FlagConvertToInt64<K>(K sValue) where K : Enum
        {
            return (Int64)Convert.ToInt64(sValue);
        }

        public static Enum StringConvertToFlag(Type sType, string sValue)
        {
            if (!sType.IsEnum)
            {
                throw new ArgumentException("Enum expected.", "TEnum");
            }
            if (sType.GetCustomAttributes(typeof(FlagsAttribute), false).Length == 0)
            {
                throw new ArgumentException("Enum expected. Must have FlagsAttribute", "TEnum");
            }

            if (string.IsNullOrEmpty(sValue) == false)
            {
                return (Enum)Enum.Parse(sType, sValue);
            }
            else
            {
                return (Enum)Activator.CreateInstance(sType);
            }
        }

        public static string FlagConvertToString(Enum sValue)
        {
            return sValue.ToString();
        }

        public static Enum Int64ConvertToFlag(Type sType, Int64 sValue)
        {
            if (!sType.IsEnum)
            {
                throw new ArgumentException("Enum expected.", "TEnum");
            }
            if (sType.GetCustomAttributes(typeof(FlagsAttribute), false).Length == 0)
            {
                throw new ArgumentException("Enum expected. Must have FlagsAttribute", "TEnum");
            }
            return (Enum)Enum.ToObject(sType, sValue);
        }

        public static Int64 FlagConvertToInt64(Enum sValue)
        {
            return (Int64)Convert.ToInt64(sValue);
        }

        public static Enum Int32ConvertToFlag(Type sType, Int32 sValue)
        {
            if (!sType.IsEnum)
            {
                throw new ArgumentException("Enum expected.", "TEnum");
            }
            if (sType.GetCustomAttributes(typeof(FlagsAttribute), false).Length == 0)
            {
                throw new ArgumentException("Enum expected. Must have FlagsAttribute", "TEnum");
            }
            return (Enum)Enum.ToObject(sType, sValue);
        }

        public static Int32 FlagConvertToInt32(Enum sValue)
        {
            return (Int32)Convert.ToInt32(sValue);
        }
    }
}