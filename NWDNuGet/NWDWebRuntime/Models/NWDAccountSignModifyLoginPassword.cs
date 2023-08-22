using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignModifyLoginPassword
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignModifyLoginPassword) + "_JS";

    [ValidateNever] 
    public string AccountSignModifyLoginPasswordName { set; get; } = string.Empty;
    
    [Display(Name = "Actual login")]
    [Required()]
    public string AccountSignModifyLoginPasswordLogin { set; get; } = string.Empty;

    [Display(Name = "Actual password")]
    [Required()]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignModifyLoginPasswordPassword { set; get; } = string.Empty;

    [Display(Name = "New login")]
    [Required()]
    public string AccountSignModifyLoginPasswordNewLogin { set; get; } = string.Empty;

    [Display(Name = "New password")]
    [Required()]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignModifyLoginPasswordNewPassword { set; get; } = string.Empty;

    [Display(Name = "Confirm")]
    [Required()]
    [Compare(nameof(AccountSignModifyLoginPasswordNewPassword))]
    public string AccountSignModifyLoginPasswordNewPasswordConfirm { set; get; } = string.Empty;

    public string AccountSignModifyLoginPasswordController { set; get; } = string.Empty;
    public string AccountSignModifyLoginPasswordAction { set; get; } = string.Empty;
    
}