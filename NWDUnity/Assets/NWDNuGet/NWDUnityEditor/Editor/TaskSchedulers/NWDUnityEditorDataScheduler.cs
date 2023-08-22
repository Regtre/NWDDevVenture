using NWDUnityShared.Tools;

namespace NWDUnityEditor.TaskSchedulers
{
    public class NWDUnityEditorDataScheduler : NWDTaskScheduler<NWDUnityEditorDataScheduler>
    {
        private object _lock = new object();

        private bool isProcessingReccurentTasks = true;
        public override bool IsProcessingReccurentTasks
        {
            get
            {
                lock (_lock)
                {
                    return isProcessingReccurentTasks;
                }
            }
        }

        public void StartRecurrentTasks()
        {
            lock (_lock)
            {
                isProcessingReccurentTasks = true;
            }
        }

        public void StopRecurrentTasks()
        {
            lock (_lock)
            {
                isProcessingReccurentTasks = false;
            }
        }
    }
}
