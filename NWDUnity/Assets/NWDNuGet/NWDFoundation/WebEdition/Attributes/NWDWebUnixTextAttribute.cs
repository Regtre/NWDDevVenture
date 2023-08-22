using System;

// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-7.0
// create NWDWebUnixTextAttributeAdapter
// Add attribut swith case in NWDCustomValidationAttributeAdapterProvider
// Add javascript in NWDWebRuntime.js

namespace NWDFoundation.WebEdition.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class NWDWebUnixTextAttribute : NWDRegularExpressionAttribute
    {
        public const string EregPattern = @"[a-zA-Z0-9_]*";
        
        public NWDWebUnixTextAttribute() : base(EregPattern)
        {
            ErrorMessage = "Only Unix text allowed";
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