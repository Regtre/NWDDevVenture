namespace NWDFoundation.Exchanges
{
    public abstract class NWDExchangeResponse
    {
        public NWDExchangeResultCode ResultCode { get; set; } = NWDExchangeResultCode.OK;
    }
}