namespace NWDWebEditor.Services
{
    public static class NWDWebEditorRecursiveService
    {
        private static Timer? _TestTimer;
        private static int _TestCounter;
#if DEBUG
        private const int K_TestTimeSpan = 60;
#else
         private static int K_TestTimeSpan = 3600;
#endif
        //private static bool _ActivePurge;

        public static void StartTimer()
        {
            TimeSpan tStartTimeSpan = TimeSpan.Zero;
            TimeSpan tPeriodTimeSpan = TimeSpan.FromSeconds(K_TestTimeSpan);
            _TestTimer = new Timer((sE) => { RecursiveService(); }, null, tStartTimeSpan, tPeriodTimeSpan);
        }

        private static void RecursiveService()
        {
            _TestCounter++;
        }
    }
}