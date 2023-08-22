using System.ComponentModel.DataAnnotations;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignAddLoginPasswordEmail
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignAddLoginPasswordEmail) + "_JS";

    [Required()]
    [Display(Name = "Email address")]
    [EmailAddress()]
    public string AccountSignAddLoginPasswordEmailEmail { set; get; } = string.Empty;

    [Required()]
    [Display(Name = "Login")]
    public string AccountSignAddLoginPasswordEmailLogin { set; get; } = string.Empty;

    [Required()]
    [Display(Name = "Password")]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignAddLoginPasswordEmailPassword { set; get; } = string.Empty;

    [Required()]
    [Display(Name = "Confirm")]
    [Compare(nameof(AccountSignAddLoginPasswordEmailPassword))]
    public string AccountSignAddLoginPasswordEmailPasswordConfirm { set; get; } = string.Empty;

    public string AccountSignAddLoginPasswordEmailController { set; get; } = string.Empty;
    public string AccountSignAddLoginPasswordEmailAction { set; get; } = string.Empty;
}