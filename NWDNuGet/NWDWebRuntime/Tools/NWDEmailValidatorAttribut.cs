using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NWDWebRuntime.Tools;
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
public class NWDEmailValidatorAttribute : DataTypeAttribute
{
    public NWDEmailValidatorAttribute()
        : base(DataType.EmailAddress)
    {
        // Set DefaultErrorMessage not ErrorMessage, allowing user to set
        // ErrorMessageResourceType and ErrorMessageResourceName to use localized messages.
        // DefaultErrorMessage = SR.EmailAddressAttribute_Invalid;
    }
    
    public override bool IsValid(object value)
    {
        string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                         + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                         + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(value.ToString());
    }
}