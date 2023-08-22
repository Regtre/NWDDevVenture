namespace NWDStandardModels.Information
{
    public static class NWDStandardModelsInformation
    {
        public static string Description()
        {
            return typeof(NWDStandardModelsInformation).Namespace + " DLL, version " + NWDVersionDll.Version;
        }
    }
}
