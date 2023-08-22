using System;
using System.Collections.Generic;

namespace NWDUnityShared.Tools
{
    public class NWDPool<T> where T : INWDPoolElement<T>, new()
    {
        private Queue<T> AvailableElements = new Queue<T>();

        public T Get()
        {
            T rResult;
            if (AvailableElements.Count == 0)
            {
                rResult = new T();
            }
            else
            {
                rResult = AvailableElements.Dequeue();
            }
            rResult.Use(this);
            return rResult;
        }

        public void SetAvailable(T sElement)
        {
            AvailableElements.Enqueue(sElement);
        }
    }

    public class NWDPool<T, U> where T : INWDPoolElement<T, U>, new()
    {
        private Queue<T> AvailableElements = new Queue<T>();

        public T Get(U sValue)
        {
            T rResult;
            if (AvailableElements.Count == 0)
            {
                rResult = new T();
            }
            else
            {
                rResult = AvailableElements.Dequeue();
            }
            rResult.Use(this, sValue);
            return rResult;
        }

        public void SetAvailable(T sElement)
        {
            AvailableElements.Enqueue(sElement);
        }
    }
}
