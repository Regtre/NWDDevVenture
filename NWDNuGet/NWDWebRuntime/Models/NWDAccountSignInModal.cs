using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignInModal
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignInModal) + "_JS";

    [Required()]
    [Display(Name = "Email address")]
    [EmailAddress()]
    public string AccountSignInModalEmail { set; get; }= string.Empty;

    [Required()]
    [Display(Name = "Password")]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignInModalPassword { set; get; }= string.Empty;

    [Display(Name = "Remember me")] 
    public bool AccountSignInModalRememberMe { set; get; }

    public string AccountSignInModalController { set; get; }= string.Empty;
    public string AccountSignInModalAction { set; get; }= string.Empty;

    public NWDAccountSignInModal()
    {
    }

    public NWDAccountSignInModal(ViewContext sViewContext)
    { 
        AccountSignInModalController = sViewContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;
        AccountSignInModalAction = sViewContext.RouteData.Values["action"]?.ToString()?? string.Empty;
    }
}