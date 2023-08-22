using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignModifyLoginPasswordEmail
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignModifyLoginPasswordEmail) + "_JS";

    [ValidateNever]
    public string AccountSignModifyLoginPasswordEmailName { set; get; } = string.Empty;
    
    [Display(Name = "Actual email address")]
    [Required()]
    [EmailAddress()]
    public string AccountSignModifyLoginPasswordEmailEmail { set; get; } = string.Empty;

    [Display(Name = "Actual login")]
    [Required()]
    public string AccountSignModifyLoginPasswordEmailLogin { set; get; } = string.Empty;

    [Display(Name = "Actual password")]
    [Required()]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignModifyLoginPasswordEmailPassword { set; get; } = string.Empty;

    [Display(Name = "New email address")]
    [Required()]
    [EmailAddress()]
    public string AccountSignModifyLoginPasswordEmailNewEmail { set; get; } = string.Empty;

    [Display(Name = "New login")]
    [Required()]
    public string AccountSignModifyLoginPasswordEmailNewLogin { set; get; } = string.Empty;

    [Display(Name = "New password")]
    [Required()]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignModifyLoginPasswordEmailNewPassword { set; get; } = string.Empty;

    [Display(Name = "Confirm")]
    [Required()]
    [Compare(nameof(AccountSignModifyLoginPasswordEmailNewPassword))]
    public string AccountSignModifyLoginPasswordEmailNewPasswordConfirm { set; get; } = string.Empty;

    public string AccountSignModifyLoginPasswordEmailController { set; get; } = string.Empty;
    public string AccountSignModifyLoginPasswordEmailAction { set; get; } = string.Empty;
    
}