using System.Collections;

namespace NWDUnityEditor.Coroutine
{
    public static class NWDEditorCoroutineToolbox
    {
        public static NWDEditorCoroutine StartCoroutine(IEnumerator sRoutine, object sOwner = null)
        {
            return new NWDEditorCoroutine(sRoutine, sOwner);
        }
        
        public static void StopCoroutine(NWDEditorCoroutine sCoroutine)
        {
            if (sCoroutine != null)
            {
                sCoroutine.Stop();
            }
        }
    }
}