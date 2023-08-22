using NWDFoundation.Logger;
using System;
using UnityEditor;

namespace NWDUnityEditor.Tools
{
    public abstract class NWDRequestorTask
    {
        protected const int kProgressHistorySize = 3;

        public event Action<NWDRequestorTask> Callback;
        public Exception exception;
        protected int?[] ProgressList = null;
        protected int CurrentProgress = 0;
        protected NWDRequestorTaskState state;

        public NWDRequestorTaskState State
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

        public NWDRequestorTask(Action<NWDRequestorTask> sCallback)
        {
            Callback = sCallback;
            state = NWDRequestorTaskState.Waiting;
        }

        public virtual void Run ()
        {
            State = NWDRequestorTaskState.Executing;

            StartProgress();

            try
            {
                Execute();
                ExecuteCallBack();
            }
            catch (Exception e)
            {
                State = NWDRequestorTaskState.Error;
                exception = e;
                NWDLogger.Error("Error while invoking Task!", e);
                FinishProgress(Progress.Status.Failed);
                return;
            }

            FinishProgress(Progress.Status.Succeeded);
            State = NWDRequestorTaskState.Success;
        }

        protected abstract void Execute();

        protected void ExecuteCallBack()
        {
            Callback?.Invoke(this);
        }

        public virtual string ProgressName ()
        {
            return null;
        }

        public virtual string ProgressDescription()
        {
            return null;
        }

        protected void StartProgress()
        {
            lock (this)
            {
                if (!string.IsNullOrEmpty(ProgressName()))
                {
                    if (ProgressList == null)
                    {
                        ProgressList = new int?[kProgressHistorySize];
                    }

                    if (ProgressList[CurrentProgress].HasValue)
                    {
                        Progress.Remove(ProgressList[CurrentProgress].Value);
                    }

                    ProgressList[CurrentProgress] = Progress.Start(ProgressName(), ProgressDescription(), Progress.Options.Indefinite);
                }
            }
        }

        protected void FinishProgress (Progress.Status sStatus)
        {
            lock (this)
            {
                if (ProgressList != null)
                {
                    Progress.Report(ProgressList[CurrentProgress].Value, 1);
                    Progress.Finish(ProgressList[CurrentProgress].Value, sStatus);
                    if (sStatus == Progress.Status.Succeeded)
                    {
                        ProgressList[CurrentProgress] = null;
                    }

                    CurrentProgress++;
                    CurrentProgress %= kProgressHistorySize;
                }
            }
        }

        public bool IsProcessing ()
        {
            return State == NWDRequestorTaskState.Waiting || State == NWDRequestorTaskState.Executing;
        }

        ~NWDRequestorTask()
        {
            lock (this)
            {
                if (ProgressList != null)
                {
                    for (int i = 0; i < ProgressList.Length; i++)
                    {

                        if (ProgressList[i].HasValue && Progress.Exists(ProgressList[i].Value))
                        {
                            Progress.Remove(ProgressList[i].Value);
                        }
                    }
                }
            }
        }
    }
}

