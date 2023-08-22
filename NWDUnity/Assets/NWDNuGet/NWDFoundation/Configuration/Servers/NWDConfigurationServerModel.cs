using System;
using NWDFoundation.Facades;

namespace NWDFoundation.Configuration.Servers
{
    [Serializable]
    public class NWDConfigurationServerModel
    {
        #region properties

        public string Name { set; get; } = "NWD_SERVER_NAME";

        public string Ip { set; get; } = "NWD_SERVER_IP";

        public string CommitHash { set; get; } = "CI_COMMIT_SHA";

        public string Job { set; get; } = "CI_JOB_ID";

        public string PipelineDate { set; get; } = "CI_PIPELINE_DATE";
        
        public int RescueTokenDuration { set; get; } = 14400; //one day
        public float OverflowLimit { set; get; } = 0.8F;

        #endregion

        #region constructor

        public NWDConfigurationServerModel()
        {
        }

        #endregion
    }
}