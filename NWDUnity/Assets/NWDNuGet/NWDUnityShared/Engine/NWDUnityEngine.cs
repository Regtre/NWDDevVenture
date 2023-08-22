using NWDUnityShared.Initializer;
using System;
using System.Diagnostics;

namespace NWDUnityShared.Engine
{
    public class NWDUnityEngine
    {
        static public INWDUnityEngineInitializer Launcher;

        static private INWDUnityEngine instance = null;

        static public INWDUnityEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Launcher?.GetEngine();
                }
                return instance;
            }
        }

        [Conditional("UNITY_EDITOR")]
        static public void ResetInstance()
        {
            instance = null;
        }

        [Conditional("UNITY_EDITOR")]
        static public void Launch()
        {
            instance = Launcher?.GetEngine();
        }
    }
}
