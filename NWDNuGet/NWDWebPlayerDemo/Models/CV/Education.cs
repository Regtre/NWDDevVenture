namespace NWDWebPlayerDemo.Models.CV;

public class Education
{
    public string Name { set; get; } = string.Empty;
    public string Degree { set; get; }= string.Empty;

    public DateTime StartDate { set; get; } = DateTime.Now;
    public DateTime EndDate { set; get; } = DateTime.Now;
}