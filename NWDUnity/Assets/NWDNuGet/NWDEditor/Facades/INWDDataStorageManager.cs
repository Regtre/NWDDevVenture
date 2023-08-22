using NWDEditor.Exchanges;
using NWDFoundation.Exchanges;

namespace NWDEditor.Facades
{
    public interface INWDDataStorageManager
    {
        #region interfaces

        public NWDResponseEditor Process(NWDRequestEditor sRequestRuntime);

        #endregion
    }
}