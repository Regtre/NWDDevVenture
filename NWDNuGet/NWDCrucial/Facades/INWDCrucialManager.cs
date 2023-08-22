using NWDCrucial.Exchanges;

namespace NWDCrucial.Facades
{
    public interface INWDCrucialManager
    {
        #region interfaces

        public NWDResponseCrucial Process(NWDRequestCrucial sRequestRuntime);

        #endregion
    }
}