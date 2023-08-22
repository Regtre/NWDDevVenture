using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Models.Enums;
namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignUp : INWDTempData
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignUp) + "_JS";

    [Required()]
    [Display(Name = "Email address")]
    [EmailAddress()]
    public string AccountSignUpEmail { set; get; }= string.Empty;

    [Required()]
    [Display(Name = "Password")]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignUpPassword { set; get; }= string.Empty;

    [Required()]
    [Display(Name = "Confirm")]
    [Compare(nameof(AccountSignUpPassword))]
    public string AccountSignUpPasswordConfirm { set; get; }= string.Empty;

    [Display(Name = "Remember me")]
    public bool AccountSignUpRememberMe { set; get; } 

    [Display(Name = "Accept terms and conditions")]
    public bool AccountSignUpChecked { set; get; }

    public string AccountSignUpController { set; get; }= string.Empty;
    public string AccountSignUpAction { set; get; }= string.Empty;

    public NWDAccountSignUp()
    {
    }

    public NWDAccountSignUp(ViewContext sViewContext)
    {
        AccountSignUpController = sViewContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;
        AccountSignUpAction = sViewContext.RouteData.Values["action"]?.ToString() ?? string.Empty;
    }
}