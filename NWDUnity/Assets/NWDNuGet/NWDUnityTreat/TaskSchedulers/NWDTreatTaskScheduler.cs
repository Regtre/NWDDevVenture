using NWDUnityShared.TaskSchedulers;
using NWDUnityShared.Tools;

namespace NWDUnityTreat.TaskSchedulers
{
    public class NWDTreatTaskScheduler : NWDTaskScheduler<NWDTreatTaskScheduler>
    {
        static private NWDTreatTaskScheduler instance = new NWDTreatTaskScheduler();
        static public NWDTreatTaskScheduler Instance { get { return instance; } }

        public override bool IsProcessingReccurentTasks => true;
    }
}
