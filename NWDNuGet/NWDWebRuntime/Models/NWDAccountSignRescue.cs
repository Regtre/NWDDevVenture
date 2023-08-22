using System.ComponentModel.DataAnnotations;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Models.Enums;
namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignRescue : INWDTempData
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignRescue) + "_JS";
    public string AccountSignRescueToken { set; get; }= string.Empty;
    public int AccountSignRescueLimit { set; get; }= 3600;
    public string AccountSignRescueEmail { set; get; }= string.Empty;
    [Required()]
    [Display(Name = "New password")]
    [MinLength(8)]
    [MaxLength(128)]
    public string AccountSignRescuePassword { set; get; }= string.Empty;
    [Required()]
    [Display(Name = "Confirm")]
    [MinLength(8)]
    [MaxLength(128)]
    [Compare(nameof(AccountSignRescuePassword))]
    public string AccountSignRescuePasswordConfirm { set; get; }= string.Empty;

    public string AccountSignRescueController { set; get; }= string.Empty;
    public string AccountSignRescueAction { set; get; }= string.Empty;
}