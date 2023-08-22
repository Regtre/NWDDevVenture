using System.Diagnostics;
using System.Reflection;
#nullable enable
namespace NWDFoundation.Configuration
{
    public class NWDLibraryInfos
    {
        public Assembly? AssemblyDll;
        public FileVersionInfo? Information;
        public bool SetUpPage = false;
        public bool Nuget = false;
        public string Imagine = string.Empty;
        public string ImagineBis = string.Empty;
        public string Description = string.Empty;
    }
}
#nullable disable