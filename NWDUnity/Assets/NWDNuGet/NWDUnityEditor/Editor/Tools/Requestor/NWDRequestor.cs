using NWDFoundation.Logger;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;

namespace NWDUnityEditor.Tools
{
    public abstract class NWDRequestor<T> where T : NWDRequestor<T>, new()
    {
        private const int kDelay = 1000;

        static private readonly object _lock = new object();
        static private T Current = null;

        static public T GetCurrent()
        {
            lock (_lock)
            {
                if (Current == null)
                {
                    Current = new T();
                }
                Current.Start();
                return Current;
            }
        }

        private NWDRequestorState state;
        private Task task = null;
        protected List<NWDRequestorRecurrentTask> RecurrentTasks;
        protected Queue<NWDRequestorTask> Tasks;

        public NWDRequestorState State
        {
            get
            {
                lock (this)
                {
                    return state;
                }
            }
            protected set
            {
                lock (this)
                {
                    state = value;
                }
            }
        }

        public NWDRequestor()
        {
            RecurrentTasks = new List<NWDRequestorRecurrentTask>();
            Tasks = new Queue<NWDRequestorTask>();
            state = NWDRequestorState.Stopped;
        }

        public NWDRequestorTask EnqueueTask(NWDRequestorTask sTask)
        {
            lock(_lock)
            {
                Tasks.Enqueue(sTask);
                return sTask;
            }
        }

        private NWDRequestorTask DequeueTask()
        {
            lock (_lock)
            {
                if (Tasks.Count == 0)
                {
                    return null;
                }
                return Tasks.Dequeue();
            }
        }

        public int AddRecurrentTask (NWDRequestorRecurrentTask sTask)
        {
            lock (_lock)
            {
                RecurrentTasks.Add(sTask);
                return RecurrentTasks.Count - 1;
            }
        }

        public void RemoveRecurrentTask(int sIndex)
        {
            lock (_lock)
            {
                RecurrentTasks.RemoveAt(sIndex);
            }
        }

        private int ReccurentTaskCount()
        {
            lock (_lock)
            {
                return RecurrentTasks.Count;
            }
        }

        protected NWDRequestorRecurrentTask GetRecurrentTask(int sIndex)
        {
            lock (_lock)
            {
                return RecurrentTasks[sIndex];
            }
        }


        public void Start ()
        {
            if (task == null)
            {
                State = NWDRequestorState.TasksOnly;
                task = Task.Run(Run);
            }
        }

        public void StartScheduler ()
        {
            Start();
            State = NWDRequestorState.Started;
        }

        public void StopScheduler ()
        {
            State = NWDRequestorState.TasksOnly;
        }

        protected virtual async Task Run()
        {
            while (!IsCanceled())
            {
                NWDRequestorTask tTask = DequeueTask();

                while (tTask != null && !IsCanceled())
                {
                    tTask.Run();
                    tTask = DequeueTask();
                }

                int i = 0;
                while (i < ReccurentTaskCount() && !IsCanceled() && State == NWDRequestorState.Started)
                {
                    NWDRequestorRecurrentTask tRTask = GetRecurrentTask(i);
                    if (tRTask.CanExecuteNow())
                    {
                        tRTask.Run();
                    }
                    i++;
                }

                await Task.Delay(kDelay);
            }
            State = NWDRequestorState.Stopped;
        }

        protected virtual bool IsCanceled()
        {
            lock (_lock)
            {
                bool rResult = Current != this;
                return rResult;
            }
        }
    }
}

