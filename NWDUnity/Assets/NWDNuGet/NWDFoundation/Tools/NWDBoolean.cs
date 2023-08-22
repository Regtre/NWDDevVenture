namespace NWDFoundation.Tools
{
    public static class NWDBoolean
    {
        public static string ToYesNoString(this bool sValue)
        {
            return sValue ? "Yes" : "No";
        }
        
        public static string To01String(this bool sValue)
        {
            return sValue ? "1" : "0";
        }
    }
}