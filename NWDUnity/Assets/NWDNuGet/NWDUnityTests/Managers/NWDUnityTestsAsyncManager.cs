using NWDUnityShared.Facades;
using System;

namespace NWDUnityTests.Manager
{
    public class NWDUnityTestsAsyncManager : INWDUnityThreadManager
    {
        public void CallOnMainThread(Action sAction)
        {
            throw new NotImplementedException();
        }

        public void UniqueCallOnMainThread(Action sAction)
        {
            throw new NotImplementedException();
        }
    }
}
