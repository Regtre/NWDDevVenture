using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using NWDFoundation.WebEdition.Attributes;

namespace NWDWebRuntime.Tools.Validators;
public class NWDWebRegularExpressionAttributeAdapter : AttributeAdapterBase<NWDWebRegularExpressionAttribute>
    {
        public NWDWebRegularExpressionAttributeAdapter( NWDWebRegularExpressionAttribute attribute, IStringLocalizer? stringLocalizer): base(attribute, stringLocalizer)
        {
        }
        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-WebRegExp", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-WebRegExp-pattern", Attribute.Pattern);
        }
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
            => Attribute.GetErrorMessage();
    }