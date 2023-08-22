using System;

// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-7.0
// create NWDWebRegularExpressionAttributeAdapter
// Add attribut swith case in NWDCustomValidationAttributeAdapterProvider
// Add javascript in NWDWebRuntime.js
namespace NWDFoundation.WebEdition.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class NWDWebRegularExpressionAttribute : NWDRegularExpressionAttribute
    {
        public NWDWebRegularExpressionAttribute(string sPattern) : base(sPattern)
        {
            ErrorMessage = "No compatible with regular expression.";
        }

        public string GetErrorMessage()
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                return string.Empty;
            }
            return ErrorMessage;
        }
    }
}