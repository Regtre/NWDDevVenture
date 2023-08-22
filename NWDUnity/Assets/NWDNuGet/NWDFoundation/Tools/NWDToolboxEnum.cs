using System;

#nullable enable
namespace NWDFoundation.Tools
{
    public partial class NWDToolboxEnum
    {
        public static T? StringConvertToEnum<T>(string sValue) where T : Enum
        {
            if (string.IsNullOrEmpty(sValue) == false)
            {
                return (T)Enum.Parse(typeof(T), sValue);
            }
            else
            {
                return (T?)Activator.CreateInstance(typeof(T));
            }
        }

        public static string EnumConvertToString<T>(T sValue) where T : Enum
        {
            return sValue.ToString();
        }

        public static T? Int32ConvertToEnum<T>(Int32 sValue) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), sValue))
            {
                return (T)Enum.ToObject(typeof(T), sValue);
            }
            else
            {
                return (T?)Activator.CreateInstance(typeof(T));
            }
        }

        public static Int32 EnumConvertToInt32<T>(T sValue) where T : Enum
        {
            return Convert.ToInt32(sValue);
        }

        public static T? Int64ConvertToEnum<T>(Int64 sValue) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), sValue))
            {
                return (T)Enum.ToObject(typeof(T), sValue);
            }
            else
            {
                return (T?)Activator.CreateInstance(typeof(T));
            }
        }

        public static Int64 EnumConvertToInt64<T>(T sValue) where T : Enum
        {
            return Convert.ToInt64(sValue);
        }

        public static Enum? StringConvertToEnum(Type sType, string sValue)
        {
            if (!sType.IsEnum)
            {
                throw new ArgumentException("Type expected.");
            }
            if (string.IsNullOrEmpty(sValue) == false)
            {
                return (Enum)Enum.Parse(sType, sValue);
            }
            else
            {
                return (Enum?)Activator.CreateInstance(sType);
            }
        }

        public static string EnumConvertToString(Enum sValue)
        {
            return sValue.ToString();
        }

        public static Enum? Int32ConvertToEnum(Type sType, Int32 sValue)
        {
            if (!sType.IsEnum)
            {
                throw new ArgumentException("Type expected.");
            }
            if (Enum.IsDefined(sType, sValue))
            {
                return (Enum)Enum.ToObject(sType, sValue);
            }
            else
            {
                return (Enum?)Activator.CreateInstance(sType);
            }
        }

        public static Int32 EnumConvertToInt32(Enum sValue)
        {
            return Convert.ToInt32(sValue);
        }

        public static Enum? Int64ConvertToEnum(Type sType, Int64 sValue)
        {
            if (!sType.IsEnum)
            {
                throw new ArgumentException("Type expected.");
            }
            if (Enum.IsDefined(sType, sValue))
            {
                return (Enum)Enum.ToObject(sType, sValue);
            }
            else
            {
                return (Enum?)Activator.CreateInstance(sType);
            }
        }

        public static Int64 EnumConvertToInt64(Enum sValue)
        {
            return Convert.ToInt64(sValue);
        }
    }
}
#nullable disable