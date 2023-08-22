namespace NWDWebStandard.Services
{
    public static class NWDWebStandardRecursiveService
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
            TestConfiguration();
            TimeSpan tStartTimeSpan = TimeSpan.Zero;
            TimeSpan tPeriodTimeSpan = TimeSpan.FromSeconds(K_TestTimeSpan);
            _TestTimer = new Timer((sE) => { RecursiveService(); }, null, tStartTimeSpan, tPeriodTimeSpan);
        }

        private static void RecursiveService()
        {
            //NWDWebEditorAdminCreation.CreateAdminAccount();
            _TestCounter++;
            TestConfiguration();
        }

        private static void TestConfiguration()
        {
            
        }
    }
}