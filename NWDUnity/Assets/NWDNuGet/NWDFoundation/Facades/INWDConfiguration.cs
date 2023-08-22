using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Facades
{
    public interface INWDConfiguration
    {
        #region interfaces
        public void  PrepareAfterConfiguration();
        public bool IsLoaded();
        public bool SetUpPage { set; get; }

        public void RandomFake();

        #endregion
    }
}