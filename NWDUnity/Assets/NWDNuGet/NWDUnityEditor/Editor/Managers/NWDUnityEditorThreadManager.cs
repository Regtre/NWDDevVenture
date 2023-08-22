using NWDUnityShared.Facades;
using System;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Managers
{
    public class NWDUnityEditorThreadManager : INWDUnityThreadManager
    {
        public object _lock = new object();
        private event Action Methods;
        private Action Update;

        public NWDUnityEditorThreadManager()
        {
            EditorApplication.update -= ProcessUpdate;
            EditorApplication.update += ProcessUpdate;
        }

        public void CallOnMainThread(Action sAction)
        {
            lock (_lock)
            {
                if (sAction != null)
                {
                    Methods += sAction;

                    Update -= Call;
                    Update += Call;
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

                    Update -= Call;
                    Update += Call;
                }
            }
        }

        private void Call()
        {
            lock (_lock)
            {
                Update -= Call;
                Action tSafety = Methods;
                Methods = null;
                tSafety?.Invoke();
            }
        }

        private void ProcessUpdate ()
        {
            lock (_lock)
            {
                Update?.Invoke();
            }
        }
    }
}
