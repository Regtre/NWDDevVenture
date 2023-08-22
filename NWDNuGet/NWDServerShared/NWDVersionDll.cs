namespace NWDServerShared
{
    public abstract class NWDVersionDll
    {
        public static string Version { set; get; } = "1.1.181"; //VERSION
        public static bool NuGet { set; get; } = false; //NUGET
        public static string Description { set; get; } = "This module contains common concepts for servers of services.";
        public static string Imagine { set; get; } = "An octopus giving keys";
        public static string ImagineBis { set; get; } = "An octopus giving papers and books";
    }
}