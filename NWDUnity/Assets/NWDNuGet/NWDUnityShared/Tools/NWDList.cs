using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NWDUnityShared.Tools
{
    public class NWDList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
    {
        private object _lock = new object();

        private List<T> _list;

        public T this[int index]
        {
            get
            {
                lock (_lock)
                {
                    return _list[index];
                }
            }
            set
            {
                lock ( _lock)
                {
                    _list[index] = value;
                }
            }
        }

        object IList.this[int index]
        {
            get
            {
                lock (_lock)
                {
                    return ((IList)_list)[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    ((IList)_list)[index] = value;
                }
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _list.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                lock (_lock)
                {
                    return ((IList<T>)_list).IsReadOnly;
                }
            }
        }

        public bool IsSynchronized
        {
            get
            {
                lock (_lock)
                {
                    return ((IList)_list).IsSynchronized;
                }
            }
        }

        public object SyncRoot
        {
            get
            {
                lock (_lock)
                {
                    return ((IList)_list).SyncRoot;
                }
            }
        }

        public bool IsFixedSize
        {
            get
            {
                lock (_lock)
                {
                    return ((IList)_list).IsFixedSize;
                }
            }
        }

        public void Add(T item)
        {
            lock (_lock)
            {
                _list.Add(item);
            }
        }

        public int Add(object value)
        {
            lock (_lock)
            {
                return ((IList)_list).Add(value);
            }
        }

        public void Clear()
        {
            lock(_lock)
            {
                _list.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _list.Contains(item);
            }
        }

        public bool Contains(object value)
        {
            lock(_lock)
            {
                return ((IList)_list).Contains(value);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_lock)
            {
                _list.CopyTo(array, arrayIndex);
            }
        }

        public void CopyTo(Array array, int index)
        {
            lock (_lock)
            {
                ((IList)_list).CopyTo(array, index);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_lock)
            {
                return _list.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (_lock)
            {
                return _list.IndexOf(item);
            }
        }

        public int IndexOf(object value)
        {
            lock(_lock)
            {
                return ((IList)_list).IndexOf(value);
            }
        }

        public void Insert(int index, T item)
        {
            lock (_lock)
            {
                _list.Insert(index, item);
            }
        }

        public void Insert(int index, object value)
        {
            lock (_lock)
            {
                ((IList)_list).Insert(index, value);
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _list.Remove(item);
            }
        }

        public void Remove(object value)
        {
            lock (_lock)
            {
                ((IList)_list).Remove(value);
            }
        }

        public void RemoveAt(int index)
        {
            lock(_lock)
            {
                _list.RemoveAt(index);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_lock)
            {
                return ((IList)_list).GetEnumerator();
            }
        }

        public int FindIndex(Predicate<T> sMatch)
        {
            lock(_lock)
            {
                return _list.FindIndex(sMatch);
            }
        }

        public List<T> ToList()
        {
            lock (_lock)
            {
                return _list;
            }
        }

        public NWDList()
        {
            _list = new List<T>();
        }

        public NWDList(List<T> sList)
        {
            _list = sList;
        }
    }
}
