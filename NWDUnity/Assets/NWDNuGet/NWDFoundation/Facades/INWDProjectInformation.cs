using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Facades
{
    public interface INWDProjectInformation
    {
        #region interfaces

        public string GetProjectInformationInstanceName();
        public ulong GetProjectId();
        public NWDEnvironmentKind GetProjectEnvironment();

        #endregion
    }
}