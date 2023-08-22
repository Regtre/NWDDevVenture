using NWDFoundation.Models;

namespace NWDWebPlayerDemo.Models.CV;

public class CV : NWDPlayerData
{
    public string Title { set; get; } = string.Empty;
    public string Name { set; get; }= string.Empty;
    public string Surname { set; get; }= string.Empty;
    public string Email { set; get; }= string.Empty;
    public string Phone { set; get; }= string.Empty;
    public string Address { set; get; }= string.Empty;
    public DateTime Birthdate { set; get; } = DateTime.Now;
    public string City { set; get; }= string.Empty;
    public string Country { set; get; }= string.Empty;
    public string ZipCode { set; get; }= string.Empty;
    public string Linkedin { set; get; }= string.Empty;
    public string Github { set; get; }= string.Empty;
    public string Website { set; get; }= string.Empty;
    public string About { set; get; }= string.Empty;
    public string Image { set; get; }= string.Empty;
    public List<Experience> Experiences { set; get; } = new List<Experience>();
    public List<Education> Educations { set; get; } = new List<Education>();
    public List<Skill> Skills { set; get; } = new List<Skill>();
    public List<Language> Languages { set; get; } = new List<Language>();
    public List<Interest> Interests { set; get; } = new List<Interest>(); 
}
