namespace NWDWebRuntime.Tools;

public class TimeTools
{
    public static string GetStringTimeAgo(DateTime sDateTime)
    {
        string rReturn = string.Empty;
        
        TimeSpan tTimeSpan = DateTime.Now - sDateTime;
        if(tTimeSpan.TotalSeconds < 60)
        {
            rReturn = tTimeSpan.Seconds + " seconds ago";
        }
        else if(tTimeSpan.TotalMinutes < 60)
        {
            rReturn = tTimeSpan.Minutes + " minutes ago";
        }
        else if(tTimeSpan.TotalHours < 24)
        {
            rReturn = tTimeSpan.Hours + " hours ago";
        }
        else if(tTimeSpan.TotalDays < 7)
        {
            rReturn = tTimeSpan.Days + " days ago";
        }
        else if(tTimeSpan.TotalDays < 30)
        {
            rReturn = tTimeSpan.Days / 7 + " weeks ago";
        }
        else if(tTimeSpan.TotalDays < 365)
        {
            rReturn = tTimeSpan.Days / 30 + " months ago";
        }
        else
        {
            rReturn = tTimeSpan.Days / 365 + " years ago";
        }
        return rReturn; 
    }
}