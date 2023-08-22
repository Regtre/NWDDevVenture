using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignInMini
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignInMini) + "_JS";

    [Required()]
    [Display(Name = "Email address")]
    [EmailAddress()]
    public string AccountSignInMiniEmail { set; get; } = string.Empty;

    [Required()]
    [Display(Name = "Password")]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignInMiniPassword { set; get; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool AccountSignInMiniRememberMe { set; get; }

    public string AccountSignInMiniController { set; get; } = string.Empty;
    public string AccountSignInMiniAction { set; get; } = string.Empty;

    public NWDAccountSignInMini()
    {
    }

    public NWDAccountSignInMini(ViewContext sViewContext)
    {
        AccountSignInMiniController = sViewContext.RouteData.Values["controller"]?.ToString()?? string.Empty;
        AccountSignInMiniAction = sViewContext.RouteData.Values["action"]?.ToString() ?? string.Empty;
    }
}