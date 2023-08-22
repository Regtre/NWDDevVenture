namespace NWDWebStandard.Models.Enums
{
    public enum NWDStatisticsConsolidateRange
    {
#if DEBUG
        ThisMinute,
#endif
        ThisHour,
        ThisDate,
        ThisMonth,
        Day,
        Date,
        Hour,
        Month,
        Year,
    }
}