using NWDUnityShared.Tools;

namespace NWDUnityRuntime.TaskSchedulers
{
    public class NWDUnityRuntimeDataScheduler : NWDTaskScheduler<NWDUnityRuntimeDataScheduler>
    {
        public override bool IsProcessingReccurentTasks => false;
    }
}
