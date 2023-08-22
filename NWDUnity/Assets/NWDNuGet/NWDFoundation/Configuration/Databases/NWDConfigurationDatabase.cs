using System;
using System.Collections.Generic;
using NWDFoundation.Facades;

namespace NWDFoundation.Configuration.Databases
{
    [Serializable]
    public class NWDConfigurationDatabase : INWDConfiguration
    {
        #region static properties
        /// <summary>
        /// Shared instance
        /// </summary>
        public static NWDConfigurationDatabase KConfig = new NWDConfigurationDatabase();
        public  static bool Loaded { set; get; } = false;
        #endregion

        #region properties
        /// <summary>
        /// List of database for Account studio 
        /// </summary>
        public List<NWDDatabaseCredentials> DatabaseAccountArray { set; get; } = new List<NWDDatabaseCredentials>() {};
        
        /// <summary>
        /// List of databases for data players
        /// </summary>
        public List<NWDDatabaseCredentials> DatabasePlayerArray { set; get; } = new List<NWDDatabaseCredentials>() {};

        /// <summary>
        /// List of database for data studio 
        /// </summary>
        public List<NWDDatabaseCredentials> DatabaseStudioArray { set; get; } = new List<NWDDatabaseCredentials>() {};


        public void PrepareAfterConfiguration()
        {
            
        }

        public bool IsLoaded()
        {
            return Loaded;
        }

        public bool SetUpPage { set; get; } = false;

        public void RandomFake()
        {
            
        }
        
        #endregion
    }
}