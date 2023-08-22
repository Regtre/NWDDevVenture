namespace NWDServerFront
{
    public abstract class NWDVersionDll
    {
        public static string Version { set; get; } = "1.1.181"; //VERSION
        public static bool NuGet { set; get; } = false; //NUGET
        //TODO : Enter a description for mid-journey prompt here (/imagine)
        public static string Description { set; get; } = "This module contains tools for front of services.";
        public static string Imagine { set; get; } = "A chameleon giving a book to counter";
        public static string ImagineBis { set; get; } = "A chameleon giving a big package to a counter";
    }
}