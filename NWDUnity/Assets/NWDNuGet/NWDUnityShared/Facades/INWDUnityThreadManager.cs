using System;

namespace NWDUnityShared.Facades
{
    public interface INWDUnityThreadManager
    {
        /// <summary>
        /// Calls a function form the main thread.<br/>
        /// Note: There is no gagrantee on when the function will be called.
        /// </summary>
        /// <param name="sAction">The function to call.</param>
        public void CallOnMainThread(Action sAction);

        /// <summary>
        /// Calls a function form the main thread only once until it has been executed.<br/>
        /// It simply prevents a function to be called several times at once when the main thread processes the functions.<br/>
        /// Note: There is no gagrantee on when the method will be called.<br/>
        /// </summary>
        /// <param name="sAction">The function to call.</param>
        public void UniqueCallOnMainThread(Action sAction);
    }
}
