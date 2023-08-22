using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NWDWebRuntime.Tools
{
    public static class EnumExtensions
    {
        public static string? GetDisplayName(this Enum sEnu)
        {
            var tAttr = GetDisplayAttribute(sEnu);
            return tAttr.Name;
        }

        public static string? GetDescription(this Enum sEnu)
        {
            var tAttr = GetDisplayAttribute(sEnu);
            return tAttr.Description;
        }

        private static DisplayAttribute GetDisplayAttribute(object sValue)
        {
            Type tType = sValue.GetType();
            if (!tType.IsEnum)
            {
                throw new ArgumentException($"Type {tType} is not an enum");
            }
            // Get the enum field.
            var tField = tType.GetField(sValue.ToString() ?? string.Empty);
            return (tField == null ? null : tField.GetCustomAttribute<DisplayAttribute>())!;
        }
    }
}
