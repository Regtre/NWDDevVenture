using NWDWebRuntime.Models;

namespace NWDWebRuntime.Managers;

public class NWDNotificationManager{
    
    public static List<NWDNotification> GetNotificationForAccount(ulong AccountReference)
    {
        Dictionary<string,string> tDico = new Dictionary<string, string> { { nameof(NWDNotification.AccountReference), AccountReference.ToString() } };
        return NWDWebDBDataManager.GetBy<NWDNotification>(tDico);

    }
    public static List<NWDNotification> GetNotificationForAccountSortByDateDesc(ulong AccountReference)
    {

        List<NWDNotification> tNotifications = GetNotificationForAccount(AccountReference);
        tNotifications.Sort();
        return tNotifications;

    }
    
    public static void Save(NWDNotification? sNotification)
    {
        NWDWebDBDataManager.SaveData<NWDNotification>(sNotification);
    }

    public static NWDNotification? GetByReference(ulong Reference)
    {
        return NWDWebDBDataManager.GetDataByReference<NWDNotification>(Reference);
    }
}