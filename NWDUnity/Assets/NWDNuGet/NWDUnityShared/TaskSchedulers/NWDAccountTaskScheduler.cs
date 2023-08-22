using NWDUnityShared.Tools;
using System.Threading.Tasks;

namespace NWDUnityShared.TaskSchedulers
{
    public class NWDAccountTaskScheduler : NWDTaskScheduler<NWDAccountTaskScheduler>
    {
        public override bool IsProcessingReccurentTasks => true;
    }
}
