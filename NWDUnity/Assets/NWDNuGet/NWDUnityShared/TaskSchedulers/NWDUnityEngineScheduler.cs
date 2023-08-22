using NWDUnityShared.Tools;

namespace NWDUnityShared.TaskSchedulers
{
    public class NWDUnityEngineScheduler : NWDTaskScheduler<NWDUnityEngineScheduler>
    {
        public override bool IsProcessingReccurentTasks => false;
    }
}
