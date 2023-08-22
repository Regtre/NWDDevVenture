using NWDTreat.Exchanges;
namespace NWDTreat.Facades
{
    public interface INWDTreatManager
    {
        #region interfaces
        public NWDResponseTreat Process(NWDRequestTreat sRequestRuntime);

        #endregion
    }
}