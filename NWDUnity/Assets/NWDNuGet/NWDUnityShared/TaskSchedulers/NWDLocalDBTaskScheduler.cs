using NWDUnityShared.Tools;

namespace NWDUnityShared.TaskSchedulers
{
    public class NWDLocalDBTaskScheduler : NWDTaskScheduler<NWDLocalDBTaskScheduler>
    {
        public override bool IsProcessingReccurentTasks => false;
    }
}
