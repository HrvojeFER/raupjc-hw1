using System;

namespace hw1
{
    
    public class IntegerList : IIntegerList
    {

        private int[] _internalStorage;

        public int Count { get; private set; }

        public IntegerList()
        {
            _internalStorage = new int[4];
            Count = 0;
        }

        public IntegerList(int initialSize)
        {
            if (initialSize < 0)
                throw new ArgumentException("Initial size must be positive.");

            _internalStorage = new int[initialSize];
            Count = 0;
        }

        public void Add(int item)
        {
            if (_internalStorage.Length == Count)
            {
                int[] tmp = new int[_internalStorage.Length << 1];
                _internalStorage.CopyTo(tmp, 0);
                _internalStorage = tmp;
            }

            _internalStorage[Count++] = item;
        }

        public bool Remove(int item)
        {
            for (int index = 0 ; index < Count ; ++index)
                if (_internalStorage[index] == item)
                    return RemoveAt(index);

            return false;
        }

        public bool RemoveAt(int index)
        {
            if (index > Count || index < 0)
                throw new IndexOutOfRangeException();

            for (int i = index + 1; i < Count; i++)
                _internalStorage[i - 1] = _internalStorage[i];

            --Count;
            return true;
        }

        public int GetElement(int index)
        {
            if (index > Count || index < 0)
                throw new IndexOutOfRangeException();

            return _internalStorage[index];
        }

        public int IndexOf(int item)
        {
            for (int index = 0 ; index < Count ; ++index)
                if (_internalStorage[index] == item)
                    return index;

            return -1;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(int item)
        {
            for (int index = 0; index < Count; ++index)
                if (_internalStorage[index] == item) return true;

            return false;
        }
        
    }
    
}
