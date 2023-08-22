using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Facades
{
    public interface INWDTreatKey
    {
        #region interfaces

        public string GetTreatInstanceName();
        public string GetTreatKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind);

        #endregion
    }
}