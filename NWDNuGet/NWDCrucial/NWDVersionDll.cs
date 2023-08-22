namespace NWDCrucial
{
    public abstract class NWDVersionDll
    {
        public static string Version { set; get; } = "1.1.181"; //VERSION
        public static bool NuGet { set; get; } = false; //NUGET
        public static string Description { set; get; } = "This module store credentials to communicate between the project's hub and the services servers. This module allows secure exchanges between the project's hub and services servers. It allows the realization of projects in a form usable by Net-Worked-Data.";
        public static string Imagine { set; get; } = "A iron safe";
        public static string ImagineBis { set; get; } = "A Wild West type safe";
    }
}