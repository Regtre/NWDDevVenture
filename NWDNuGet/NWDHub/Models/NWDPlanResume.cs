
using NWDFoundation.Models;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDPlanResume
    {
        public NWDPlan Plan = NWDPlan.Standard;
        public bool Connected = true;
        public NWDPlanCluster Cluster = NWDPlanCluster.NetWorkedData;
        public bool CustomModels = false;

        public string LicencePrice = string.Empty;
        public string ClusterPrice= string.Empty;

        static private Dictionary<NWDPlan, NWDPlanResume> _PlanDictionary = new Dictionary<NWDPlan, NWDPlanResume>();

        static public NWDPlanResume GetPlanResume(NWDPlan sPlan)
        {
            if (_PlanDictionary.ContainsKey(sPlan))
            {
                return _PlanDictionary[sPlan];
            }
            else
            {
                NWDPlanResume tPlan = new NWDPlanResume();
                tPlan.Initialisation(sPlan);
                _PlanDictionary.Add(sPlan, tPlan);
                return tPlan;
            }
        }

        protected void Initialisation(NWDPlan sPlan)
        {
            Plan = sPlan;
            switch (Plan)
            {
                case NWDPlan.Disconnected:
                    Connected = false;
                    Cluster = NWDPlanCluster.None;
                    CustomModels = true;
                    LicencePrice = "free";
                    ClusterPrice = "-";
                    break;
                case NWDPlan.Standard:
                    Connected = true;
                    Cluster = NWDPlanCluster.NetWorkedData;
                    CustomModels = false;
                    LicencePrice = "4 000$/year";
                    ClusterPrice = "1 000$/10k players/year";
                    break;
                case NWDPlan.Dedicated:
                    Connected = true;
                    Cluster = NWDPlanCluster.NetWorkedDataPrivate;
                    CustomModels = false;
                    LicencePrice = "4 000$/year";
                    ClusterPrice = "1 300$/server/year";
                    break;
                case NWDPlan.Custom:
                    Connected = true;
                    Cluster = NWDPlanCluster.NetWorkedDataPrivate;
                    CustomModels = true;
                    LicencePrice = "4 000$/year";
                    ClusterPrice = "1 500$/server/year";
                    break;
                case NWDPlan.Community:
                    Connected = true;
                    Cluster = NWDPlanCluster.Another;
                    CustomModels = true;
                    LicencePrice = "4 000$/year";
                    ClusterPrice = "yours";
                    break;
                case NWDPlan.Fork:
                    Connected = true;
                    Cluster = NWDPlanCluster.Another;
                    CustomModels = true;
                    LicencePrice = "20 000$/year";
                    ClusterPrice = "yours";
                    break;
            }
        }

    }
}