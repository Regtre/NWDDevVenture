using NWDUnityRuntime.Engine;
using NWDUnityShared.Facades;
using System;

namespace NWDUnityRuntime.Managers
{
    public class NWDUnityRuntimeThreadManager : INWDUnityThreadManager
    {
        public object _lock = new object();
        private event Action Methods;

        public void CallOnMainThread(Action sAction)
        {
            lock (_lock)
            {
                if (sAction != null)
                {
                    Methods += sAction;

                    NWDUnityEngineRuntime.Instance.update -= Call;
                    NWDUnityEngineRuntime.Instance.update += Call;
                }
            }
        }
        public void UniqueCallOnMainThread(Action sAction)
        {
            lock (_lock)
            {
                if (sAction != null)
                {
                    Methods -= sAction;
                    Methods += sAction;

                    NWDUnityEngineRuntime.Instance.update -= Call;
                    NWDUnityEngineRuntime.Instance.update += Call;
                }
            }
        }

        private void Call()
        {
            NWDUnityEngineRuntime.Instance.update -= Call;
            lock (_lock)
            {
                Action tSafety = Methods;
                Methods = null;
                tSafety?.Invoke();
            }
        }
    }
}
