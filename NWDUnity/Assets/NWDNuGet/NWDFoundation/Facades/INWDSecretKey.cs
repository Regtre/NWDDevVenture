using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Facades
{
    public interface INWDSecretKey
    {
        #region interfaces

        public string GetSecretKeyInstanceName();
        public string GetSecretKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind);

        #endregion
    }
}