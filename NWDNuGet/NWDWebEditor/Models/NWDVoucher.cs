using System.ComponentModel.DataAnnotations;

namespace NWDWebEditor.Models;

public class NWDVoucher 
{
    
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { set; get; } = string.Empty;
    
    public DateTime StartDate { set; get; } = DateTime.Now;
    public DateTime EndDate { set; get; } = DateTime.Now.AddDays(7);
    public ulong Service { set; get; }
    
}