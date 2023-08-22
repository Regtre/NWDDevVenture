using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDUnityEditor.Constants;
using NWDUnityShared.Constants;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    /// <summary>
    /// Find package path class.
    /// Use the ScriptableObject to find the path of this package
    /// </summary>
    public class  NWDFindPackage : ScriptableObject
    {
        /// <summary>
        /// The script file path.
        /// </summary>
        public string ScriptFilePath;
        /// <summary>
        /// The script folder.
        /// </summary>
        public string ScriptFolder;
        /// <summary>
        /// The script folder from assets.
        /// </summary>
        public string ScriptFolderFromAssets;
        /// <summary>
        /// The shared instance.
        /// </summary>
        private static NWDFindPackage kSharedInstance;
        /// <summary>
        /// Ascencor to shared instance.
        /// </summary>
        /// <returns>The shared instance.</returns>
        public static NWDFindPackage SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = ScriptableObject.CreateInstance(typeof(NWDFindPackage).Name) as NWDFindPackage;
                if (kSharedInstance != null)
                {
                    NWDLogger.Trace("  ScriptableObject.CreateInstance(" + typeof(NWDFindPackage).Name + ") SUCCESS!");
                    kSharedInstance.ReadPaths();
                }
                else
                {
                    NWDLogger.Warning("  ScriptableObject.CreateInstance(" + typeof(NWDFindPackage).Name + ") FAILED!");
                }
            }
            return kSharedInstance;
        }

        /// <summary>
        /// Reads the paths.
        /// </summary>
        public void ReadPaths()
        {
            MonoScript tMonoScript = MonoScript.FromScriptableObject(this);
            ScriptFilePath = AssetDatabase.GetAssetPath(tMonoScript);
            FileInfo tFileInfo = new FileInfo(ScriptFilePath);
            ScriptFolder = tFileInfo.Directory.ToString();
            ScriptFolder = ScriptFolder.Replace("\\", "/");
            ScriptFolderFromAssets = NWDConstantsUnityRuntime.K_Assets + ScriptFolder.Replace(Application.dataPath, string.Empty);
        }

        /// <summary>
        /// Packages the path.
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="sAddPath">S add path.</param>
        public static string PathOfPackage(string sAddPath = NWDConstants.C_EMPTY_STRING)
        {
            return SharedInstance().ScriptFolder + sAddPath;
        }

        public static string PathEditor(string sAddPath = NWDConstants.C_EMPTY_STRING)
        {
            if (SharedInstance().ScriptFolderFromAssets == null)
            {
                return SharedInstance().ScriptFolderFromAssets + "/NWDEditor/Editor/Images/" + sAddPath;
            }
            else
            {
                return "/NWDEditor/Editor/Images/" + sAddPath;
            }
        }

        public static Texture2D PackageEditor(string sPath)
        {
            string tPath = sPath;
            if (EditorGUIUtility.isProSkin)
            {
                tPath = Path.GetFileNameWithoutExtension(sPath) + NWDConstantsUnityEditor._pro;
            }
            return _PackageEditor(tPath);
        }

        private static string[] K_EXTENSIONS = new string[] { ".png", ".psd", ".jpeg", ".jpg" , ".svg" };

        private static Texture2D _PackageEditor(string sPath)
        {
            Texture2D rTexture2D = null;
            foreach (string tExtension in K_EXTENSIONS)
            {
                string tPath = PathEditor(sPath + tExtension);
                rTexture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(tPath);
                if (rTexture2D != null)
                {
                    break;
                }
            }
            if (rTexture2D == null)
            {
                NWDLogger.Warning("Failed to find " + sPath);
            }
            else
            {
            }
            return rTexture2D;
        }

        public static Texture2D EditorTexture(string sName)
        {
            string tName = sName;
            if (EditorGUIUtility.isProSkin)
            {
                tName = sName + NWDConstantsUnityEditor._pro;
            }
            string tFileName = Path.GetFileName(tName);
            return _EditorTexture(tFileName);
        }

        private static Texture2D _EditorTexture(string sName)
        {
            Texture2D rTexture = null;
            //Debug.LogWarning(" find tFileName " + sName + "?");
            string[] sGUIDs = AssetDatabase.FindAssets(sName + " t:texture");
            foreach (string tGUID in sGUIDs)
            {
                string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                if (tPathFilename.Equals(sName))
                {
                    rTexture = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                    //Debug.LogWarning(" GOOD tFileName " + sName + " was found!");
                    break;
                }
            }
            return rTexture;
        }
    }
}

