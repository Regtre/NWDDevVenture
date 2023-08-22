using System.ComponentModel.DataAnnotations;

namespace NWDWebTreat.Models.Enums;

public enum NWDVoucherKind
{
    [Display(Name = "Date to date voucher")]
    DateToDate = 0,
    AddDuration = 1,
    
}