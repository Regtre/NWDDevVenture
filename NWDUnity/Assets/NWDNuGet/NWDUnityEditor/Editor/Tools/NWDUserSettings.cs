using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public static class NWDUserSettings
    {
        static private readonly string DataPath = Path.Combine(Application.dataPath, "../UserSettings");
        /// <summary>
        /// Get the unique user settings file path for a <see cref="Type"/>.
        /// <para>Note: If several types carry the same name they will return the same path.</para>
        /// </summary>
        /// <param name="sDataType">The <see cref="Type"/> to find the path for.</param>
        /// <returns>The found path for the <see cref="Type"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        private static string GetFilePathForType (Type sDataType)
        {
            if (sDataType.IsGenericType)
            {
                throw new ArgumentException("Cannot create NWDUserSettings from a generic type!");
            }

            string tFolder = Path.Combine(DataPath, "Net-Worked-Data3", sDataType.Name + ".json");

            return tFolder;
        }

        /// <summary>
        /// Load a user settings from the default path.
        /// </summary>
        /// <typeparam name="T">The type of the user settings.</typeparam>
        /// <returns>The settings if found, null otherwise.</returns>
        public static T Load<T> () where T : class
        {
            string tPath = GetFilePathForType (typeof (T));
            return Load<T> (tPath);
        }

        /// <summary>
        /// Load a user settings.
        /// </summary>
        /// <typeparam name="T">The type of the user settings.</typeparam>
        /// <param name="sFilePath">The full path of the user settings file.</param>
        /// <returns>The settings if found, null otherwise.</returns>
        public static T Load<T>(string sFilePath) where T : class
        {
            T rResult = null;
            string tFileContent = "";

            if (File.Exists(sFilePath))
            {
                tFileContent = File.ReadAllText(sFilePath);
            }

            if (!string.IsNullOrEmpty(tFileContent))
            {
                rResult = JsonConvert.DeserializeObject<T>(tFileContent);
            }
            return rResult;
        }

        /// <summary>
        /// Load a user settings.
        /// </summary>
        /// <typeparam name="T">The type of the user settings.</typeparam>
        /// <param name="sDefault">The default value to return if nothing was found.</param>
        /// <returns>The settings if found, the default value otherwise.</returns>
        public static T LoadOrDefault<T> (T sDefault) where T : class
        {
            T rResult = Load<T>();

            if (rResult == null)
            {
                rResult = sDefault;
            }
            return rResult;
        }

        /// <summary>
        /// Load a user settings.
        /// </summary>
        /// <typeparam name="T">The type of the user settings.</typeparam>
        /// <param name="sFilePath">The full path of the user settings file.</param>
        /// <param name="sDefault">The default value to return if nothing was found.</param>
        /// <returns>The settings if found, the default value otherwise.</returns>
        public static T LoadOrDefault<T>(string sFilePath, T sDefault) where T : class
        {
            T rResult = Load<T>(sFilePath);

            if (rResult == null)
            {
                rResult = sDefault;
            }
            return rResult;
        }

        /// <summary>
        /// Save a user settings to the default path.
        /// <para>It will override the current settings.</para>
        /// </summary>
        /// <typeparam name="T">The type of the user settings.</typeparam>
        /// <param name="sData">The data to save.</param>
        public static void Save<T> (T sData) where T : class
        {
            string tPath = GetFilePathForType(typeof(T));
            Save<T>(sData, tPath);
        }

        /// <summary>
        /// Save a user settings.
        /// <para>It will override the current settings.</para>
        /// </summary>
        /// <typeparam name="T">The type of the user settings.</typeparam>
        /// <param name="sData">The data to save.</param>
        /// <param name="sFilePath">The full path of the user settings file.</param>
        public static void Save<T>(T sData, string sFilePath) where T : class
        {
            string tFileContent = "";
            if (sData != null)
            {
                tFileContent = JsonConvert.SerializeObject(sData);
            }
            Directory.CreateDirectory(Path.GetDirectoryName(sFilePath));
            File.WriteAllText(sFilePath, tFileContent);
        }
    }
}
