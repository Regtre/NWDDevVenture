namespace NWDWebRuntime.Models
{
    public class StandardErrorViewModel
    {
        public string RequestId { set; get; } = string.Empty;

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}