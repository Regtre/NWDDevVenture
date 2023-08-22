using System.ComponentModel.DataAnnotations;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignAddLoginPassword
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignAddLoginPassword) + "_JS";

    [Required()]
    [Display(Name = "Login")]
    public string AccountSignAddLoginPasswordLogin { set; get; }= string.Empty;

    [Required()]
    [Display(Name = "Password")]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignAddLoginPasswordPassword { set; get; }= string.Empty;

    [Required()]
    [Display(Name = "Confirm")]
    [Compare(nameof(AccountSignAddLoginPasswordPassword))]
    public string AccountSignAddLoginPasswordPasswordConfirm { set; get; }= string.Empty;

    public string AccountSignAddLoginPasswordController { set; get; }= string.Empty;
    public string AccountSignAddLoginPasswordAction { set; get; }= string.Empty;
}