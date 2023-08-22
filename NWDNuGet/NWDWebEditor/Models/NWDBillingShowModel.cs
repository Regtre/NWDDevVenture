using System.ComponentModel.DataAnnotations;

namespace NWDWebEditor.Models
{
    public partial class NWDBillingShowModel
    {
        public string AccountReference { set; get; } = string.Empty;
        public string BillingReference { set; get; } = string.Empty;
        public string IndentReference { set; get; } = string.Empty;
    }
    
    public class NWDReportListModel
    {
        public List<NWDReportModel> ReportList { set; get; } = new List<NWDReportModel>();
    }

    public class NWDReportModel
    {
        public string YearString { set; get; }= string.Empty;
        public DateTime DateStart { set; get; }
        public DateTime DateEnd { set; get; }
        // public List<BillingModel> BillingList { set; get; }
    }

    [Serializable]
    public enum NWDReportChangeBilling
    {
        None = 0,
        Refund = 99,
    }

    [Serializable]
    public class NWDReportChangeBillingModel
    {
        [Display(Name = "Bill reference")]
        //[Required(ErrorMessage = "Ce champ est requis.")]
        public string Reference { set; get; } = string.Empty;
        //[Required(ErrorMessage = "Ce champ est requis.")]
        public NWDReportChangeBilling Change { set; get; }
        //[Required(ErrorMessage = "Ce champ est requis.")]
        public string YearString { set; get; }= string.Empty;
        //[Required(ErrorMessage = "Ce champ est requis.")]
        public DateTime DateStart { set; get; }
        //[Required(ErrorMessage = "Ce champ est requis.")]
        public DateTime DateEnd { set; get; }
    }

    [Serializable]
    public class NWDAddServiceModel
    {
        [Display(Name = "Account reference")]
        public string Reference { set; get; }= string.Empty;
        // [Required(ErrorMessage = "Ce champ est requis.")]
        // public AccountServiceType Service { set; get; }
        [Required(ErrorMessage = "Ce champ est requis.")]
        public int Days { set; get; }
    }

    
}