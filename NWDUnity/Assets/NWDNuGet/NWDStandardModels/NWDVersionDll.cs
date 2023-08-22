namespace NWDStandardModels
{
    public abstract class NWDVersionDll
    {
        public static string Version { set; get; } = "1.1.181"; //VERSION
        public static bool NuGet { set; get; } = false; //NUGET
        public static string Description { set; get; } = "This module contains standard models from the project editor hub website.";
        public static string Imagine { set; get; } = "A rabbit character drawing comic characters on white paperboard";
        public static string ImagineBis { set; get; } = "A fox drawing with a compass on a huge blueprint";
    }
}