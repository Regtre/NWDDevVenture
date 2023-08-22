using System.ComponentModel.DataAnnotations;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountDelete
{
    public const string K_JavaScriptFunction = nameof(NWDAccountDelete) + "_JS";

    public const string K_Security = "Yes! Delete my account!";

    public NWDAccountDelete(string sAccountDeleteSentence, string sAccountDeleteController, string sAccountDeleteAction)
    {
        AccountDeleteSentence = sAccountDeleteSentence;
        AccountDeleteController = sAccountDeleteController;
        AccountDeleteAction = sAccountDeleteAction;
    }

    [Required(ErrorMessage = "Security sentence required")]
    [Display(Name = "Enter the security sentence!")]
    public string AccountDeleteSentence { set; get; }
    public string AccountDeleteController { set; get; }
    public string AccountDeleteAction { set; get; }
}