namespace NWDFoundation.Information
{
    public static class NWDFoundationInformation
    {
        public static string Description()
        {
            return typeof(NWDFoundationInformation).Namespace + " DLL, version " + NWDVersionDll.Version;
        }
    }
}
