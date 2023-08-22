using System;

namespace NWDDevServerMiddleBack.Tools
{
    public static class TimeTools
    {
        public static int DateTimeToTimestampUnix(DateTime sDateTime)
        {
            return (int)(sDateTime.Subtract(DateTime.UnixEpoch)).TotalSeconds;
        }

        public static int GetNowTimestampUnix()
        {
            return (int)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalSeconds;
        }

        public static int GetNowPlusNMinutesTimestampUnix(int minutes)
        {
            return (int)(DateTime.UtcNow.AddMinutes(minutes).Subtract(DateTime.UnixEpoch)).TotalSeconds;
        }

        public static DateTime TimeStampToDateTime(int sTimeStamp)
        {
            return DateTime.UnixEpoch.AddSeconds(sTimeStamp);
        }

        public static int ConvertDaysToSeconds(int days)
        {
            return days * 86400;
        }

        public static Boolean isTimeStampGreaterThanNow(string timeStamp)
        {
            DateTime now = DateTime.UtcNow;
            DateTime time = TimeStampToDateTime(Int32.Parse(timeStamp));
            if (DateTime.Compare(now, time) <= 0) return true;
            return false;
        }
    }
}