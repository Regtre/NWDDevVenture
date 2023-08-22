using NWDFoundation.Information;

namespace NWDUnityEditor.Information
{
    public static class NWDUnityEditorInformation
    {
        public static NWDReleaseVersion Version = NWDVersionDll.Version;

        public static string Description()
        {
            return typeof(NWDUnityEditorInformation).Namespace +" DLL, version " + Version.ToString();
        }
    }
}
