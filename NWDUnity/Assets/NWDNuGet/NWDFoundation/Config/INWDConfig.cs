using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades;

namespace NWDFoundation.Config
{
    /// <summary>
    /// Interface allowing to create classes which will be used as configurators in particular environments.
    /// </summary>
    public interface INWDConfig : INWDProjectKey
    {
        /*
        #region interface

        /// <summary>
        /// All Environments in project
        /// </summary>
        /// <returns></returns>
        public NWDEnvironment[] ReturnAllEnabledEnvironments();

        public NWDEnvironment[] ReturnAllDisabledEnvironments();

        public NWDEnvironment ReturnActiveEnvironment();

        */
        //public string ServerURL();

        //public string WebSiteURL();

        public string GetDefaultWebEditor();
        public string WebEditorURL();
        //public string WebServerURLFormat();
        public ulong GetProjectId();       
        public NWDExchangeDevice GetDeviceOS();
        public NWDDataTrackDescription GetSelectedEnvironment();
        /*
        /// <summary>
        /// Active environment in build or runtime
        /// </summary>
        /// <returns></returns>
        //public NWDEnvironment ReturnActiveEnvironment();

        public void Prepare();

        /// <summary>
        /// Save this config
        /// </summary>
        public void Save();

        /// <summary>
        /// Create all datamanager by environment
        /// </summary>
        public INWDDatabaseConnection CreateDatabasesConnectionEnvironment(NWDEnvironment sEnvironment);

        /// <summary>
        /// Allows the use of a serializer by config
        /// </summary>
        /// <param name="sObject"></param>
        /// <returns></returns>
        public string ToJson(Object sObject);

        #endregion
        */
    }
}