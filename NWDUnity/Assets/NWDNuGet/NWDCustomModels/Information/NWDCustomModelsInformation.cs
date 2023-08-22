namespace NWDCustomModels.Information
{
    public static class NWDCustomModelsInformation
    {
        public static string Description()
        {
            return typeof(NWDCustomModelsInformation).Namespace + " DLL, version " + NWDVersionDll.Version;
        }
    }
}
