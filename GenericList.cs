using System;

namespace GenericListSol
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
    }

}
