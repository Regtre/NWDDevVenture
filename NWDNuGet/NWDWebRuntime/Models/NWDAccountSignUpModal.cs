using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignUpModal
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignUpModal) + "_JS";

    [Required()]
    [Display(Name = "Email address")]
    [EmailAddress()]
    public string AccountSignUpModalEmail { set; get; } = string.Empty;

    [Required()]
    [Display(Name = "Password")]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignUpModalPassword { set; get; } = string.Empty;

    [Required()]
    [Display(Name = "Confirm")]
    [Compare(nameof(AccountSignUpModalPassword))]
    public string AccountSignUpModalPasswordConfirm { set; get; } = string.Empty;

    [Display(Name = "Remember me")] public bool AccountSignUpModalRememberMe { set; get; }

    [Display(Name = "Accept terms and conditions")]
    public bool AccountSignUpModalChecked { get; set;}

    public string AccountSignUpModalController { set; get; } = string.Empty;
    public string AccountSignUpModalAction { set; get; } = string.Empty;

    public NWDAccountSignUpModal()
    {
    }

    public NWDAccountSignUpModal(ViewContext sViewContext)
    {
        AccountSignUpModalController = sViewContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;
        AccountSignUpModalAction = sViewContext.RouteData.Values["action"]?.ToString() ?? string.Empty;
    }
}