// Add validator for NWDWebNoHtmlAttribute
$.validator.addMethod("WebNoHtml", function (value, element, params) {
    var match;
    if (this.optional(element)) {
        return true;
    }
    match = new RegExp(params).exec(value);
    return (match && (match.index === 0) && (match[0].length === value.length));
});
$.validator.unobtrusive.adapters.addSingleVal("WebNoHtml", "pattern");
// Add validator for NWDWebUnixTextAttribute
$.validator.addMethod("UnixText", function (value, element, params) {
    var match;
    if (this.optional(element)) {
        return true;
    }
    match = new RegExp(params).exec(value);
    return (match && (match.index === 0) && (match[0].length === value.length));
});
$.validator.unobtrusive.adapters.addSingleVal("UnixText", "pattern");

// Add validator for NWDWebRegularExpressionAttribute
$.validator.addMethod("WebRegExp", function (value, element, params) {
    var match;
    if (this.optional(element)) {
        return true;
    }
    match = new RegExp(params).exec(value);
    return (match && (match.index === 0) && (match[0].length === value.length));
});
$.validator.unobtrusive.adapters.addSingleVal("WebRegExp", "pattern");