using System;
using System.Collections;
using System.Collections.Generic;

namespace GenericListEnumerator
{
    public class GenericList <T> : IGenericList <T>
    {
        private T[] _internalStorage;

        public int Count { get; private set; }

        public GenericList()
        {
            _internalStorage = new T[4];
            Count = 0;
        }

        public GenericList(int initialSize)
        {
            if (initialSize < 0)
                throw new ArgumentException("Initial size must be positive.");

            _internalStorage = new T[initialSize];
            Count = 0;
        }

        public void Add(T item)
        {
            if (_internalStorage.Length == Count)
            {
                var tmp = new T[_internalStorage.Length << 1];
                _internalStorage.CopyTo(tmp, 0);
                _internalStorage = tmp;
            }

            _internalStorage[Count++] = item;
        }

        public bool Remove(T item)
        {
            for (var index = 0; index < Count; ++index)
                if (_internalStorage[index].Equals(item))
                    return RemoveAt(index);

            return false;
        }

        public bool RemoveAt(int index)
        {
            if (index > Count || index < 0)
                throw new IndexOutOfRangeException();

            for (var i = index + 1; i < Count; i++)
                _internalStorage[i - 1] = _internalStorage[i];

            --Count;
            return true;
        }

        public T GetElement(int index)
        {
            if (index > Count || index < 0)
                throw new IndexOutOfRangeException();

            return _internalStorage[index];
        }

        public int IndexOf(T item)
        {
            for (var index = 0; index < Count; ++index)
                if (_internalStorage[index].Equals(item))
                    return index;

            return -1;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(T item)
        {
            for (var index = 0; index < Count; ++index)
                if (_internalStorage[index].Equals(item)) return true;

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new GenericListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public class GenericListEnumerator<T> : IEnumerator<T>
    {
        private T[] _internalStorage;
        private int _index;
        private bool _disposed;

        object IEnumerator.Current => Current;

        public T Current { get; private set; }

        public GenericListEnumerator(IGenericList<T> list)
        {
            _internalStorage = new T[list.Count];
            for (var index = 0; index < list.Count; ++index)
                _internalStorage[index] = list.GetElement(index);
            
            _index = -1;
            _disposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _internalStorage = null;
                }
            }
            _disposed = true;
        }

        public bool MoveNext()
        {
            ++_index;
            if (_index == _internalStorage.Length)
            {
                Reset();
                return false;
            }

            Current = _internalStorage[_index];

            return true;
        }

        public void Reset()
        {
            _index = 0;
            Current = _internalStorage[_index];
        }

    }

}
