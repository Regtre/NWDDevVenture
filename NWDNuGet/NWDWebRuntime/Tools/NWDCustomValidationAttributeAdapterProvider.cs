using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using NWDFoundation.WebEdition.Attributes;
using NWDWebRuntime.Tools.Validators;

namespace NWDWebRuntime.Tools;

public class NWDCustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly IValidationAttributeAdapterProvider baseProvider = new ValidationAttributeAdapterProvider();

    public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
    {
        if (attribute is NWDWebNoHtmlAttribute tWebNoHtmlAttribute)
        {
            return new NWDWebNoHtmlAttributeAdapter(tWebNoHtmlAttribute, stringLocalizer);
        }
        
        if (attribute is NWDWebUnixTextAttribute tWebUnixTextAttribute)
        {
            return new NWDWebUnixTextAttributeAdapter(tWebUnixTextAttribute, stringLocalizer);
        }
        
        if (attribute is NWDWebRegularExpressionAttribute tWebRegularExpressionAttribute)
        {
            return new NWDWebRegularExpressionAttributeAdapter(tWebRegularExpressionAttribute, stringLocalizer);
        }

        return baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }
}