using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using NWDFoundation.WebEdition.Attributes;

namespace NWDWebRuntime.Tools.Validators;
public class NWDWebNoHtmlAttributeAdapter : AttributeAdapterBase<NWDWebNoHtmlAttribute>
    {
        public NWDWebNoHtmlAttributeAdapter( NWDWebNoHtmlAttribute attribute, IStringLocalizer? stringLocalizer): base(attribute, stringLocalizer)
        {
        }
        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-WebNoHtml", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-WebNoHtml-pattern", Attribute.Pattern);
        }
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
            => Attribute.GetErrorMessage();
    }