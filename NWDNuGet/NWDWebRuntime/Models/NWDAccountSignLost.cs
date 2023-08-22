using System.ComponentModel.DataAnnotations;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Models.Enums;
namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignLost : INWDTempData
{
    public const string K_JavaScriptFunction = nameof(NWDAccountSignLost) + "_JS";

    [DataType(DataType.EmailAddress)]
    [Required()]
    [Display(Name = "Email address")]
    [EmailAddress()]
    public string AccountSignLostEmail { set; get; } = string.Empty;
    public string AccountSignLostController { set; get; } = string.Empty;
    public string AccountSignLostAction { set; get; } = string.Empty;
}