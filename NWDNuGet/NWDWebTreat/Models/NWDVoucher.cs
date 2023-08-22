using System.ComponentModel.DataAnnotations;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Facades;
using NWDWebTreat.Models.Enums;

namespace NWDWebTreat.Models;

// very important to use nullable in model validation ! add #nullable disable
#nullable disable

public class NWDVoucher : INWDCaptcha
{
    [Required()]
    [Display(Name = "Emails addresses separated by comma")]
    // [EmailAddress()]
    [RegularExpression(@"^[\W]*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4}[\W]*,{1}[\W]*)*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4})[\W]*$", ErrorMessage = "Emails addresses separated by comma!")]
    public string Email { set; get; } = string.Empty;
    [Display(Name = "Message")]
    public string Message { set; get; } = string.Empty;
    [Display(Name = "Alert on using")]
    public string Alert { set; get; } = string.Empty;
    [Display(Name = "Alert Style")] 
    public NWDBootstrapKindOfStyle AlertStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
    [Required()]
    [Display(Name = "Kind of voucher")]
    public NWDVoucherKind Kind { set; get; } = NWDVoucherKind.DateToDate;
    
    [Display(Name = "Add duration in day")]
    public int Duration { set; get; } = 1;
    
    [Required()]
    [Display(Name = "Start date")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-M-d}")]
    public DateTime StartDate { set; get; } = DateTime.UtcNow;
    [Required()]
    [Display(Name = "End date")][DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-M-d}")]
    public DateTime EndDate { set; get; } = DateTime.UtcNow.AddDays(7);
    [Required()]
    [Display(Name = "Service")]
    public long Service { set; get; }
    [Required()]
    [Display(Name = "Captcha")]
    public string CaptchaValue { set; get; } = string.Empty;
    
    public List<NWDVoucher> IndividualVoucher()
    {
        List<NWDVoucher> rReturn = new List<NWDVoucher>();
        Email = Email.Replace(",", " ");
        Email = Email.Replace(";", " ");
        Email = Email.Replace("/", " ");
        foreach (string tEmail in Email.Split(" "))
        {
           string tEmailClean = tEmail.Trim(' ');
            if (string.IsNullOrEmpty(tEmailClean) == false)
            {
                rReturn.Add(new NWDVoucher()
                {
                    Email = tEmailClean,
                    Message = Message,
                    Alert = Alert,
                    AlertStyle = AlertStyle,
                    Kind = Kind,
                    Duration = Duration,
                    StartDate  =StartDate,
                    EndDate = EndDate,
                    Service = Service,
                    CaptchaValue = CaptchaValue,
                });
            }
        }

        return rReturn;
    }
}