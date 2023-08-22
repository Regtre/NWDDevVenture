#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using NWDFoundation.Logger;

namespace NWDFoundation.Configuration
{
    public static class NWDLibrariesInstalled
    {
        private static readonly Dictionary<string, NWDLibraryInfos> LibrariesNameList = new Dictionary<string, NWDLibraryInfos>();
        public static readonly List<NWDLibraryInfos> LibrariesInfoList = new List<NWDLibraryInfos>();
        public static readonly List<Assembly> AssemblyList = new List<Assembly>();

        public static void AddAssemblyByType(Type sType, bool sSetUpPage = false)
        {
            Assembly tAssembly = sType.Assembly;
            AssemblyList.Add(tAssembly);
            bool tNuGet = false;
            string? tImagine = string.Empty;
            string? tImagineBis = string.Empty;
            string? tDescription = string.Empty;
            FileVersionInfo tFileVersionInfo = FileVersionInfo.GetVersionInfo(tAssembly.Location);
            if (string.IsNullOrEmpty(tFileVersionInfo.ProductName) == false)
            {
                if (LibrariesNameList.ContainsKey(tFileVersionInfo.ProductName) == false)
                {
                    string tVersionDllName = tFileVersionInfo.ProductName + "." + nameof(NWDFoundation.NWDVersionDll);
                    Type? tTypeVersionDll = tAssembly.GetType(tVersionDllName);
                    if (tTypeVersionDll != null)
                    {
                        PropertyInfo? tPropInfo = tTypeVersionDll.GetProperty(nameof(NWDFoundation.NWDVersionDll.NuGet), BindingFlags.Public | BindingFlags.Static);
                        if (tPropInfo != null)
                        {
                            tNuGet = (bool)(tPropInfo.GetValue(null) ?? false);
                        }

                        PropertyInfo? tPropInfoImagine = tTypeVersionDll.GetProperty(nameof(NWDFoundation.NWDVersionDll.Imagine), BindingFlags.Public | BindingFlags.Static);
                        if (tPropInfoImagine != null)
                        {
                            tImagine = (string)(tPropInfoImagine.GetValue(null) ?? string.Empty);
                        }

                        PropertyInfo? tPropInfoImagineBis = tTypeVersionDll.GetProperty(nameof(NWDFoundation.NWDVersionDll.ImagineBis), BindingFlags.Public | BindingFlags.Static);
                        if (tPropInfoImagineBis != null)
                        {
                            tImagineBis = (string)(tPropInfoImagineBis.GetValue(null) ?? string.Empty);
                        }

                        PropertyInfo? tPropInfoDescription = tTypeVersionDll.GetProperty(nameof(NWDFoundation.NWDVersionDll.Description), BindingFlags.Public | BindingFlags.Static);
                        if (tPropInfoDescription != null)
                        {
                            tDescription = (string)(tPropInfoDescription.GetValue(null) ?? string.Empty);
                        }
                    }
                    else
                    {
                        NWDLogger.Error("Assembly not found = " + tFileVersionInfo.ProductName + "  => " + tVersionDllName + " for type " + sType.FullName);
                    }

                    // if (tNuGet)
                    // {
                    //     NWDLogger.TraceSuccess("Assembly " + tFileVersionInfo.ProductName + " is imported by NuGet!");
                    // }
                    // else
                    // {
                    //     NWDLogger.TraceSuccess("Assembly " + tFileVersionInfo.ProductName + " is imported by csproj!");
                    // }
                    NWDLibraryInfos tInfos = new NWDLibraryInfos() { AssemblyDll = tAssembly, Information = tFileVersionInfo, Nuget = tNuGet, Imagine = tImagine, ImagineBis = tImagineBis, Description = tDescription, SetUpPage = sSetUpPage };
                    LibrariesNameList.Add(tFileVersionInfo.ProductName, tInfos);
                    LibrariesInfoList.Add(tInfos);
                }
            }
        }

        public static List<NWDLibraryInfos> GetFileVersionInfoList()
        {
            return new List<NWDLibraryInfos>(LibrariesInfoList);
        }

        public static NWDLibraryInfos? GetFileVersionInfo(Type sType, bool sSetUpPage = false)
        {
            NWDLibraryInfos? rReturn = null;
            Assembly tAssembly = sType.Assembly;
            FileVersionInfo tFileVersionInfo = FileVersionInfo.GetVersionInfo(tAssembly.Location);
            if (string.IsNullOrEmpty(tFileVersionInfo.ProductName) == false)
            {
                if (LibrariesNameList.ContainsKey(tFileVersionInfo.ProductName) == true)
                {
                    rReturn = LibrariesNameList[tFileVersionInfo.ProductName];
                }
                else
                {
                    rReturn = new NWDLibraryInfos() { Information = tFileVersionInfo, SetUpPage = sSetUpPage };
                    LibrariesNameList.Add(tFileVersionInfo.ProductName, rReturn);
                    LibrariesInfoList.Add(rReturn);
                }
            }

            return rReturn;
        }
    }
}
#nullable disable