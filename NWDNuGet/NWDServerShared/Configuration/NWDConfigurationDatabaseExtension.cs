using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Logger;

namespace NWDServerShared.Configuration
{
    public static class NWDConfigurationDatabaseExtension
    {
        #region extensions
        /// <summary>
        /// Load the value from a file named configurationDatabase.json
        /// </summary>
        /// <param name="sObject"></param>
        /// <param name="sConfigurationManager"></param>
        public static void LoadConfiguration(this NWDConfigurationDatabase sObject, ConfigurationManager sConfigurationManager)
        {
            if (NWDConfigurationDatabase.Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDConfigurationDatabase)));
            }
            else
            {
                NWDConfigurationDatabase.Loaded = true;
                try
                {
                    sConfigurationManager.AddJsonFile(nameof(NWDConfigurationDatabase), true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }

                sObject.LoadSection(sConfigurationManager);
            }
        }

        /// <summary>
        /// Load the value from a section from  ConfigurationerverEmail.json
        /// </summary>
        /// <param name="sObject"></param>
        /// <param name="sConfigurationRoot"></param>
        public static void LoadSection(this NWDConfigurationDatabase sObject, IConfigurationRoot sConfigurationRoot)
        {
            NWDLibrariesInstalled.AddAssemblyByType(sObject.GetType());
            try
            {
                NWDConfigurationDatabase tConfig = sConfigurationRoot.GetSection(nameof(NWDConfigurationDatabase)).Get<NWDConfigurationDatabase>();
                if (tConfig != null)
                {
                    sObject.DatabaseAccountArray = tConfig.DatabaseAccountArray;
                    sObject.DatabasePlayerArray = tConfig.DatabasePlayerArray;
                    sObject.DatabaseStudioArray = tConfig.DatabaseStudioArray;
                    NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDConfigurationDatabase)));
                    //NWDLogger.Trace(nameof(NWDConfigurationDatabase), NWDLogger.SplitObjectSerializable(tConfig));

                    // NWDLogger.Trace(" ------------------ DATABASES FOR ACCOUNT ------------------");
                    // foreach (NWDDatabaseCredentials tCredentials in sObject.DatabaseAccountArray)
                    // {
                    //     NWDLogger.Trace(tCredentials.Name +" >>>>>>> "+tCredentials.Server+" "+tCredentials.Port+" "+tCredentials.Database+" "+tCredentials.User+" "+tCredentials.Secure);
                    // }
                    // NWDLogger.Trace(" ------------------ DATABASES FOR PLAYER ------------------");
                    // foreach (NWDDatabaseCredentials tCredentials in sObject.DatabasePlayerArray)
                    // {
                    //     NWDLogger.Trace(tCredentials.Name +" >>>>>>> "+tCredentials.Server+" "+tCredentials.Port+" "+tCredentials.Database+" "+tCredentials.User+" "+tCredentials.Secure);
                    // }
                    // NWDLogger.Trace(" ------------------ DATABASES FOR STUDIO ------------------");
                    // foreach (NWDDatabaseCredentials tCredentials in sObject.DatabaseStudioArray)
                    // {
                    //     NWDLogger.Trace(tCredentials.Name +" >>>>>>> "+tCredentials.Server+" "+tCredentials.Port+" "+tCredentials.Database+" "+tCredentials.User+" "+tCredentials.Secure);
                    // }
                    // NWDLogger.Trace(" ------------------ --------------------- ------------------");
                    //
                }
                else
                {
                    NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDConfigurationDatabase)));
                    NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDConfigurationDatabase)), NWDLogger.SplitObjectSerializable(new NWDConfigurationDatabase()));
                }
            }
            catch (Exception tException)
            {
                NWDLogger.Exception(tException);
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDConfigurationDatabase)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDConfigurationDatabase)), NWDLogger.SplitObjectSerializable(new NWDConfigurationDatabase()));
            }
        }

        #endregion
    }
}