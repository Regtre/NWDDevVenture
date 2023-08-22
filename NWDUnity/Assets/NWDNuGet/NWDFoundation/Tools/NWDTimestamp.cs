using System;

namespace NWDFoundation.Tools
{
    public static class NWDTimestamp
    {
        [Obsolete("Use: long NWDToolbox.TimeStamp(DateTime) instead")]
        public static int ToTimestampUnix(DateTime sDateTime)
        {
            return (int)sDateTime.Subtract(DateTime.UnixEpoch).TotalSeconds;
        }

        [Obsolete("Use: long NWDToolbox.TimeStamp(DateTime) instead")]
        public static Int64 ToTimestamp(DateTime sDateTime)
        {
            return (Int64)sDateTime.Subtract(DateTime.UnixEpoch).TotalSeconds;
        }

        [Obsolete("Use: long NWDToolbox.TimeStamp() instead")]
        public static int GetTimestampUnix()
        {
            return (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        }

        [Obsolete("Use: long NWDToolbox.TimeStampToDateTime(long) instead")]
        public static DateTime TimestampUnixToDatetime(int sTimeStamp)
        {
            return DateTime.UnixEpoch.AddSeconds(sTimeStamp);
        }

        /// <summary>
        /// Return timestamp from unix 1970 january 01.
        /// </summary>
        public static long Timestamp(DateTime sDateTime)
        {
            return ((DateTimeOffset)sDateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Return timestamp milliseconds from unix 1970 january 01.
        /// </summary>
        public static long TimestampMilliseconds(DateTime sDateTime)
        {
            return ((DateTimeOffset)sDateTime).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Return timestamp from unix 1970 january 01.
        /// </summary>
        public static long Timestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        /// <summary>
        /// Return DateTime from timestamp unix 1970 january 01.
        /// </summary>
        /// <returns>The timestamp to date time.</returns>
        /// <param name="sTimeStamp">timestamp.</param>
        public static DateTime TimeStampToDateTime(long sTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(sTimeStamp).DateTime;
        }

        /// <summary>
        /// Return DateTime from timestamp unix 1970 january 01.
        /// </summary>
        /// <returns>The timestamp to date time.</returns>
        /// <param name="sTimeStampMilliseconds">timestamp.</param>
        public static DateTime TimeStampMillisecondsToDateTime(long sTimeStampMilliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(sTimeStampMilliseconds).DateTime;
        }
    }
}