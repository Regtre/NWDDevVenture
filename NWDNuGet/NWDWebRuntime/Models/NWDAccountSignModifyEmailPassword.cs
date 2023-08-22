using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignModifyEmailPassword
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignModifyEmailPassword) + "_JS";
    
    [ValidateNever]
    public string AccountSignModifyEmailPasswordName { set; get; } = string.Empty;
    
    [Display(Name = "Actual email address")]
    [Required()]
    [EmailAddress()]
    public string AccountSignModifyEmailPasswordEmail { set; get; } = string.Empty;

    [Display(Name = "Actual password")]
    [Required()]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignModifyEmailPasswordPassword { set; get; } = string.Empty;

    [Display(Name = "New email address")]
    [Required()]
    [EmailAddress()]
    public string AccountSignModifyEmailPasswordNewEmail { set; get; } = string.Empty;

    [Display(Name = "New password")]
    [Required()]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignModifyEmailPasswordNewPassword { set; get; } = string.Empty;

    [Display(Name = "Confirm")]
    [Required()]
    [Compare(nameof(AccountSignModifyEmailPasswordNewPassword))]
    public string AccountSignModifyEmailPasswordNewPasswordConfirm { set; get; } = string.Empty;

    public string AccountSignModifyEmailPasswordController { set; get; } = string.Empty;
    public string AccountSignModifyEmailPasswordAction { set; get; } = string.Empty;
}