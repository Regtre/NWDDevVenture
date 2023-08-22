using System.Runtime.InteropServices.JavaScript;

namespace NWDWebRuntime.Models;

public class NWDNotificationFilteredLists
{
    public List<NWDNotification> TodaysNotifications { get; set; } = new List<NWDNotification>();
    public List<NWDNotification> EarliersNotifications { get; set; } = new List<NWDNotification>();


    public NWDNotificationFilteredLists() { }
    public NWDNotificationFilteredLists(List<NWDNotification> sNotifications)
    {
        TodaysNotifications = new List<NWDNotification>();
        EarliersNotifications = new List<NWDNotification>();
        foreach (NWDNotification tNotification in sNotifications)
        {
            if (tNotification.CreationDate.Date == DateTime.Now.Date)
            {
                TodaysNotifications.Add(tNotification);
            }
            else
            {
                EarliersNotifications.Add(tNotification);
            }
        }
    }

    public bool IsUnreadNotifications()
    {
        return TodaysNotifications.Any(sItem => sItem.Read == false) || EarliersNotifications.Any(sItem => sItem.Read == false);
    }

    public List<NWDNotification> GetModalNotification()
    {
        List<NWDNotification> rList = new List<NWDNotification>();
        rList.AddRange(TodaysNotifications.Where(sItem => sItem.ModalId != string.Empty));
        rList.AddRange(EarliersNotifications.Where(sItem => sItem.ModalId != string.Empty));
        return rList; 
    }
}