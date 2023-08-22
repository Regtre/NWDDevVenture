using NWDUnityShared.Engine;
using NWDUnityShared.Initializer;

namespace NWDUnityTests.Initializer
{
    public class NWDEngineInitializerTests : INWDUnityEngineInitializer
    {
        static private NWDEngineInitializerTests instance = null;
        static private INWDUnityEngineInitializer Swapped;

        private INWDUnityEngine Engine;

        public NWDEngineInitializerTests(INWDUnityEngine sEngine)
        {
            Engine = sEngine;
        }

        public INWDUnityEngine GetEngine()
        {
            return Engine;
        }

        static public void StartTests(INWDUnityEngine sEngine)
        {
            if (instance == null)
            {
                instance = new NWDEngineInitializerTests(sEngine);
                Swapped = NWDUnityEngine.Launcher;
                NWDUnityEngine.Launcher = instance;
                NWDUnityEngine.ResetInstance();
            }
        }

        static public void StopTests ()
        {
            if (instance != null)
            {
                NWDUnityEngine.Launcher = Swapped;
                NWDUnityEngine.ResetInstance();
                instance = null;
            }
        }
    }
}

