using System.ComponentModel.DataAnnotations;

namespace NWDWebEditor.Models
{

    [Serializable]
    public partial class NWDFindAccountByEmailModel
    {
        [Display(Name = "Account reference")] public string Reference { set; get; } = string.Empty;

        [Required(ErrorMessage = "Ce champ est requis.")]
        public string Email { set; get; } = string.Empty;
    }
}