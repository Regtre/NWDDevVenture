using System.ComponentModel.DataAnnotations;

namespace NWDWebRuntime.Models
{

    [Serializable]
    public class NWDAccountSignAddEmailPassword
    {
        public const string K_JavaScriptFunction = nameof(NWDAccountSignAddEmailPassword) + "_JS";

        [Required()]
        [Display(Name = "Email address")]
        [EmailAddress()]
        public string AccountSignAddEmailPasswordEmail { set; get; } = string.Empty;

        [Required()]
        [Display(Name = "Password")]
        [MinLength(8)]
        [MaxLength(128)]
        public string AccountSignAddEmailPasswordPassword { set; get; } = string.Empty;

        [Required()]
        [Display(Name = "Confirm")]
        [Compare(nameof(AccountSignAddEmailPasswordPassword))]
        public string AccountSignAddEmailPasswordPasswordConfirm { set; get; } = string.Empty;

        public string AccountSignAddEmailPasswordController { set; get; } = string.Empty;
        public string AccountSignAddEmailPasswordAction { set; get; } = string.Empty;
    }
}