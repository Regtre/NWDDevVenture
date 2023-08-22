using ByteSizeLib;
using NWDFoundation.Logger;
using NWDWebDownloader.Configuration;
using NWDWebDownloader.Models;

namespace NWDWebDownloader.Managers
{
    public class NWDDownloadManager
    {
        public static readonly Dictionary<string, Dictionary<string,List<NWDBuild>>> BuildsByConfig= new Dictionary<string, Dictionary<string,List<NWDBuild>>>();
        private static Dictionary<string,List<string>> _Directories = new Dictionary<string, List<string>>();
        private static NWDDownloadManager? _Instance;
        public static Dictionary<string,DownloadPageResource> ResourcesByConfig = new Dictionary<string, DownloadPageResource>();

        public static List<string> GetDirectories(NWDDownloadConfig? sConfig)
        {
            if (sConfig != null)
            {
                UpdateDirectories(sConfig);
                _Directories[sConfig.PageName].Sort();
                return _Directories[sConfig.PageName];
            }
            else
            {
                return new List<string>();
            }
        }

        public static void UpdateDirectories(NWDDownloadConfig sConfig)
        {
            if (!_Directories.ContainsKey(sConfig.PageName)) { _Directories.Add(sConfig.PageName, new List<string>() { string.Empty }); }
            DirectoryInfo tDirectoryInfo = new DirectoryInfo(sConfig.RootPath);
            foreach (DirectoryInfo tDirectory in tDirectoryInfo.GetDirectories())
            {
                if(!_Directories[sConfig.PageName].Contains(tDirectory.Name)){_Directories[sConfig.PageName].Add(tDirectory.Name);}
            }
        }
        public static void PrepareBuilds(NWDDownloadConfig? sConfig)
        {
            if (sConfig != null)
            {
                if (!BuildsByConfig.ContainsKey(sConfig.PageName))
                {
                    BuildsByConfig.Add(sConfig.PageName,new Dictionary<string,List<NWDBuild>>());
                    ResourcesByConfig.Add(sConfig.PageName,new DownloadPageResource());
                }
                else
                {
                    BuildsByConfig[sConfig.PageName].Clear();
                    ResourcesByConfig[sConfig.PageName].Clear();
                }
                WalkDirectoryTree(sConfig.PageName , new DirectoryInfo(sConfig.RootPath), BuildsByConfig[sConfig.PageName],ResourcesByConfig[sConfig.PageName],sConfig);
                SortList(sConfig,BuildsByConfig[sConfig.PageName]);
                UpdateDirectories(sConfig);
            }
        }

        private static void WalkDirectoryTree(string sConfigPageName, DirectoryInfo sRoot,Dictionary<string,List<NWDBuild>> sBuilds,DownloadPageResource sDownloadPageResource,NWDDownloadConfig sConfig)
        {
            FileInfo[]? tFiles = null;
            DirectoryInfo[]? tSubDirs = null;

            try
            {
                tFiles = sRoot.GetFiles("*.*");
            }
            catch (Exception tException)
            {
                NWDLogger.Exception(tException);
            }

            if (tFiles != null)
            {
                foreach (FileInfo tFileInfo in tFiles)
                {
                    string? tDirectory = Path.GetFileName(tFileInfo.DirectoryName); 
                    if (tDirectory!= null && !sBuilds.ContainsKey(tDirectory))
                    {
                        sBuilds.Add(tDirectory, new List<NWDBuild>());
                    }

                    NWDBuild tBuild = new NWDBuild(sConfigPageName, tFileInfo.Name, tFileInfo.FullName, tFileInfo.LastWriteTime, ByteSize.FromBytes(tFileInfo.Length).ToString(), tFileInfo.Extension);
                    
                    SaveFileinCache(sBuilds, sDownloadPageResource, sConfig, tDirectory, tBuild, tFileInfo);
                   
                }
                tSubDirs = sRoot.GetDirectories();
                foreach (DirectoryInfo tDirectoryInfo in tSubDirs)
                {
                    WalkDirectoryTree(sConfigPageName, tDirectoryInfo,sBuilds,sDownloadPageResource,sConfig);
                }
            }
        }

        private static void SaveFileinCache(Dictionary<string, List<NWDBuild>> sBuilds, DownloadPageResource sDownloadPageResource,
            NWDDownloadConfig sConfig, string? sDirectory, NWDBuild sBuild, FileInfo sFileInfo)
        {
            if (sDirectory != null)
            {
                if (!sBuilds[sDirectory].Contains(sBuild))
                {
                    if (sFileInfo.Name.Equals(sConfig.HeaderFileName))
                    {
                        sDownloadPageResource.Header = File.ReadAllText(sFileInfo.FullName);
                    }
                    else if (sFileInfo.Name.Equals(sConfig.FooterFileName))
                    {
                        sDownloadPageResource.Footer = File.ReadAllText(sFileInfo.FullName);
                    }
                    else if (sFileInfo.Name.Equals(sConfig.DescriptionFileName))
                    {
                        if (sDownloadPageResource.DescriptionByCategory.ContainsKey(sDirectory))
                        {
                            sDownloadPageResource.DescriptionByCategory[sDirectory] = File.ReadAllText(sFileInfo.FullName);
                        }
                        else
                        {
                            sDownloadPageResource.DescriptionByCategory.Add(sDirectory, File.ReadAllText(sFileInfo.FullName));
                        }
                    }
                    else
                    {
                        sBuilds[sDirectory].Add(sBuild);
                    }
                }
            }
        }

        private static void SortList(NWDDownloadConfig sConfig,Dictionary<string,List<NWDBuild>> sBuilds)
        {
            foreach (KeyValuePair<string, List<NWDBuild>> tBuildsByDirectory in sBuilds)
            {
                sBuilds[tBuildsByDirectory.Key] = tBuildsByDirectory.Value.OrderByDescending(sItem => File.GetLastWriteTime(sItem.Path)).ToList();
            }
        }

        public static NWDDownloadManager GetInstance()
        {
            if (_Instance == null) _Instance = new NWDDownloadManager();
            return _Instance;
        }
    }
}