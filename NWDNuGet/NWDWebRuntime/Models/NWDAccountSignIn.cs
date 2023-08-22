using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Tools;

using NWDWebRuntime.Models.Enums;
namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignIn : INWDTempData
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignIn) + "_JS";

    [Required()]
    [EmailAddress()]
    //[NWDEmailValidator]
    //[RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "Email is not valid")]
    [Display(Name = "Email_address")]
    public string AccountSignInEmail { set; get; } = string.Empty;

    [Required()]
    [PasswordPropertyText()]
    [MinLength(8)]
    [MaxLength(128)]
    [Display(Name = "Password")]
    public string AccountSignInPassword { set; get; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool AccountSignInRememberMe { set; get; } = false;

    public string AccountSignInController { set; get; } = string.Empty;
    public string AccountSignInAction { set; get; } = string.Empty;

    public NWDAccountSignIn()
    {
    }

    public NWDAccountSignIn(ViewContext sViewContext)
    {
        AccountSignInController = sViewContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;
        AccountSignInAction = sViewContext.RouteData.Values["action"]?.ToString() ?? string.Empty;
    }
}