using System;

namespace NWDUnityShared.Tools
{
    public interface INWDPoolElement<T> : IDisposable where T : INWDPoolElement<T>, new()
    {
        public void Use(NWDPool<T> sPool);
    }

    public interface INWDPoolElement<T, U> : IDisposable where T : INWDPoolElement<T, U>, new()
    {
        public void Use(NWDPool<T, U> sPool, U sValue);
    }
}
