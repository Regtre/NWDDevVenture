using NWDUnityRuntime.Engine;
using NWDUnityShared.Engine;
using NWDUnityShared.Initializer;
using UnityEngine;

namespace NWDUnityRuntime.Initializer
{
    public class NWDUnityRuntimeEngineInitializer : INWDUnityEngineInitializer
    {
#if !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static public void OnGameLaunch()
        {
            NWDUnityEngine.Launcher = new NWDUnityRuntimeEngineInitializer();
        }
#endif

        public INWDUnityEngine GetEngine()
        {
            return NWDUnityEngineRuntime.Instance;
        }
    }
}