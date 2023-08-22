using System;

// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-7.0
// create NWDWebNoHtmlAttributeAdapter
// Add attribut swith case in NWDCustomValidationAttributeAdapterProvider
// Add javascript in NWDWebRuntime.js
namespace NWDFoundation.WebEdition.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class NWDWebNoHtmlAttribute : NWDRegularExpressionAttribute
    {
        public const string EregPattern = @"^(?!.*<[^>]+>).*";
        public NWDWebNoHtmlAttribute() : base(EregPattern)
        {
            ErrorMessage = "No html tags allowed";
        }

        public string GetErrorMessage()
        {
            //TODO get localized string
            
            return ErrorMessage;
        }
    }
}