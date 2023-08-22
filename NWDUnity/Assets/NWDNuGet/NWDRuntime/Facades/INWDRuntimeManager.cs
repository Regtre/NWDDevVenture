using NWDFoundation.Exchanges;
using NWDRuntime.Exchanges;

namespace NWDRuntime.Facades
{
    public interface INWDRuntimeManager
    {
        #region interfaces

        public NWDResponseRuntime Process(NWDRequestRuntime sRequestRuntime);

        #endregion
    }
}