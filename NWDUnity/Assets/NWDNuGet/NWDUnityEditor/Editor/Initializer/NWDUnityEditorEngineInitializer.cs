using NWDUnityEditor.Engine;
using NWDUnityRuntime.Engine;
using NWDUnityShared.Engine;
using NWDUnityShared.Initializer;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Initializer
{
    public class NWDUnityEditorEngineInitializer : INWDUnityEngineInitializer
    {
        private const bool RuntimeTest = false;

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static public void OnGameLaunch()
        {
            NWDUnityEngine.Launcher = new NWDUnityEditorEngineInitializer();
            NWDUnityEngine.Launch();
        }
#endif

        public INWDUnityEngine GetEngine()
        {
            INWDUnityEngine rResult = NWDUnityEngineEditor.Instance;
            if (RuntimeTest)
            {
                rResult = NWDUnityEngineRuntime.Instance;
            }
            return rResult;
        }
    }
}