namespace NWDServerHumanInterface
{
    public abstract class NWDVersionDll
    {
        public static string Version { set; get; } = "1.1.181"; //VERSION
        public static bool NuGet { set; get; } = false; //NUGET
        public static string Description { set; get; } = "This module contains tools for human interface front of services.";
        public static string Imagine { set; get; } = "A mouse giving a huge paper";
        public static string ImagineBis { set; get; } = "An elephant giving a small paper";
    }
}