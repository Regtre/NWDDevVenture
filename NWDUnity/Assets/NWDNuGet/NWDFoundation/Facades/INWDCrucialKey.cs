using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Facades
{
    public interface INWDCrucialKey
    {

        public string GetCrucialInstanceName();
        
        #region interfaces
        public ulong GetCrucialProjectId();
        public NWDEnvironmentKind GetCrucialEnvironment();
        public string GetCrucialKey();
        public string GetCrucialProjectKey();
        public string GetCrucialSecretKey();

        #endregion
    }
}