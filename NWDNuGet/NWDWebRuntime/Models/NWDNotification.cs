using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDWebRuntime.Models;

public class NWDNotification : NWDDatabaseWebBasicModel, IComparable
{
    public string Body { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string Icon { get; set; } = string.Empty;
    
    public bool Read { get; set; } = false;
    public ulong AccountReference { get; set; } = 0;

    public string Link { get; set; } = string.Empty;

    public string ModalId
    {
        get;
        set;
    } = string.Empty; 
    
    [NWDWebPropertyDescription("Modal", NWDWebEditionStyle.Object,false, "", "", "")]
    public NWDModal Modal { get; set; } = new NWDModal();

    public int CompareTo(object? obj)
    {
        return -CreationDate.CompareTo((obj as NWDNotification)?.CreationDate);
    }

    public NWDNotification()
    {
        CreationDate = DateTime.Now; 
    } 
}