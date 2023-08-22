using System;
using System.Collections;

namespace NWDUnityEditor.Coroutine
{
    public class NWDEditorCoroutineWaitingFor
    {
        public DateTime End;

        public NWDEditorCoroutineWaitingFor(int sSeconds)
        {
            End = DateTime.Now.AddSeconds(sSeconds);
        }
        static public IEnumerator WaitingFor(int sSeconds)
        {
            NWDEditorCoroutineWaitingFor tDateTime = new NWDEditorCoroutineWaitingFor(sSeconds);
            while (tDateTime.End > DateTime.Now)
            {
                yield return null;
            }
        }
    }
}