using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using NVelocity.Runtime.Parser.Node;
using NWDFoundation.Tools;
using NWDWebRuntime.Facades;

// very important to use nullable in model validation ! add #nullable disable
#nullable disable

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDContactUsModel : INWDCaptcha
{
    public const string K_JavaScriptFunction = nameof(NWDContactUsModel) + "_JS";
    [Required()] 
    [Display(Name = "Name")] public string SenderName { set; get; } = string.Empty;

    [Required()]
    [EmailAddress()]
    [Display(Name = "Email address")]
    public string SenderEmail { set; get; } = string.Empty;
    
    [Display(Name = "Subject")] 
    public string Subject { set; get; }
    
    [Display(Name = "Category")] 
    public string Category { set; get; }
    
    [Display(Name = "SubCategory")] 
    public string SubCategory { set; get; }

    [Required()]
    [Display(Name = "Message")]
    //[RegularExpression()]
    public string Message { set; get; } = string.Empty;

    [Required()]
    [Display(Name = "Captcha")]
    public string CaptchaValue { set; get; } = string.Empty;
}