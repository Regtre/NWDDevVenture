using System;
namespace NWDUnityEditor.Constants
{
    public abstract class NWDEditorConstants // TODO: Make it static instead ?
    {
        // Idemobi alert Strings
        public const string K_ALERT_IDEMOBI_TITLE = "Net-Worked-Data";
        public const string K_ALERT_IDEMOBI_MESSAGE = "Net-Worked-Data is an idéMobi© package to create and manage datas in your games. You can create your owner classes and manage like standard Net-Worked-Data";
        public const string K_ALERT_IDEMOBI_OK = "Thanks!";
        public const string K_ALERT_IDEMOBI_SEE_DOC = "See online docs";
        public const string K_ALERT_IDEMOBI_SEE_WEBSITE = "See website";
        public const string K_ALERT_IDEMOBI_DOC_HTTP = "http://www.net-worked-data.com/";

        #region Automated requests delays

        public const int CheckDataRepeatEvery = 10;
        public const int UpdateDataSelectedRepeatEvery = 10;
        public const int UpdateSettings = 10;

        #endregion
    }
}
