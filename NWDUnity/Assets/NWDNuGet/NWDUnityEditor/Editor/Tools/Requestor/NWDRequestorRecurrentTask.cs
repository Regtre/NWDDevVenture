using NWDFoundation.Logger;
using System;
using UnityEditor;

namespace NWDUnityEditor.Tools
{
    public abstract class NWDRequestorRecurrentTask : NWDRequestorTask
    {
        private float Delay;
        public DateTime nextCall;
        public DateTime NextCall
        {
            get
            {
                lock (this)
                {
                    return nextCall;
                }
            }
            private set
            {
                lock (this)
                {
                    nextCall = value;
                }
            }
        }

        protected NWDRequestorRecurrentTask(float sDelay, bool sStartNow, Action<NWDRequestorTask> sCallback) : base(sCallback)
        {
            Delay = sDelay;
            state = NWDRequestorTaskState.None;

            if (sStartNow)
            {
                NextCall = DateTime.Now;
            }
            else
            {
                CalculateNextCall();
            }
        }
        public virtual bool CanExecuteNow ()
        {
            return DateTime.Now >= NextCall;
        }

        public override void Run()
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
            finally
            {
                CalculateNextCall();
            }
            FinishProgress(Progress.Status.Succeeded);
            State = NWDRequestorTaskState.None;
        }

        protected void CalculateNextCall ()
        {
            NextCall = DateTime.Now.AddSeconds(Delay);
        }

        public void ForceRun ()
        {
            State = NWDRequestorTaskState.Waiting;
            NextCall = DateTime.Now;
        }
    }
}

