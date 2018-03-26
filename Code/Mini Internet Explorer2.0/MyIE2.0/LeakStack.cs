using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

internal sealed class System_StackDebugView<T>
{
    // Fields
    private Stack<T> stack;

    // Methods
    public System_StackDebugView(Stack<T> stack)
    {
        if (stack == null)
        {
            throw new ArgumentNullException("stack");
        }
        this.stack = stack;
    }

    // Properties
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items
    {
        get
        {
            return this.stack.ToArray();
        }
    }
}



/// <summary>
/// A capacity fixed stack that when it is full the newly added item will squeeze the earliestly added item out.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable, ComVisible(false), DebuggerTypeProxy(typeof(System_StackDebugView<>)), DebuggerDisplay("Count = {Count}")]
public class LeakStack<T> : IEnumerable<T>, ICollection, IEnumerable
{
    public class LeakEventArgs:EventArgs
    {
        T _leakItem;
        int _capacity;
        int _leakIndex;

        public LeakEventArgs(T item, int capacity, int index)
        {
            _leakItem = item;
            _capacity = capacity;
            _leakIndex = index;
        }

        public int Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }

        public int LeakIndex
        {
            get { return _leakIndex; }
            set { _leakIndex = value; }
        }

        public T LeakItem
        {
            get { return _leakItem; }
            set { _leakItem = value; }
        }

    }

    public delegate void LeakHandler(object sender, LeakEventArgs e);

    // Fields
    private T[] _array;
    private int _capacity = 20;
    private static T[] _emptyArray;
    private int _size;
    private int _top;
    [NonSerialized]
    private object _syncRoot;
    private int _version;

    public EventHandler OnPushed;
    public EventHandler OnPoped;
    public LeakHandler OnLeak;

    // Methods
    static LeakStack()
    {
        LeakStack<T>._emptyArray = new T[0];
    }

    public LeakStack()
    {
        this._array = LeakStack<T>._emptyArray;
        this._size = 0;
        this._top = -1;
        this._version = 0;
    }

    public LeakStack(int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException("capacity can't be negative!");
        }
        this._array = new T[capacity];
        this._size = 0;
        this._top = -1;
        this._version = 0;
        this._capacity = capacity;
    }

    public LeakStack(IEnumerable<T> collection)
    {
        if (collection == null)
        {
            throw new ArgumentNullException();
        }
        ICollection<T> is2 = collection as ICollection<T>;
        if (is2 != null)
        {
            int count = is2.Count;
            this._array = new T[count];
            is2.CopyTo(this._array, 0);
            this._size = count;
            this._top = count - 1;
            this._capacity = count;
        }
        else
        {
            this._size = 0;
            this._array = new T[4];
            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    this.Add(enumerator.Current);
                }
                T[] localArray = new T[this._size];
                Array.Copy(this._array, 0, localArray, 0, this._size);
                this._array = localArray;
                this._capacity = this._size;
            }
        }
    }

    public void Clear()
    {
        Array.Clear(this._array, 0, this._size);
        this._size = 0;
        this._top = -1;
        this._version++;
    }

    public bool Contains(T item)
    {
        int index = this._top;
        int flag = this._size;

        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        while (flag > 0)
        {
            if (item == null)
            {
                if (this._array[index] == null)
                {
                    return true;
                }
            }
            else if ((this._array[index] != null) && comparer.Equals(this._array[index], item))
            {
                return true;
            }
            index = (this._capacity + --index) % this._capacity;
            flag--;
        }
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
        {
            throw new ArgumentNullException();
        }
        if ((arrayIndex < 0) || (arrayIndex > array.Length))
        {
            throw new ArgumentOutOfRangeException();
        }
        if ((array.Length - arrayIndex) < this._size)
        {
            throw new ArgumentException("target array don't have enough capacity!");
        }
        int index = ((this._top - this._size) + this._capacity) % this._capacity + 1;
        int flag = this._size;
        int targetIndex = arrayIndex;
        while (flag > 0)
        {
            array[targetIndex] = this._array[index];
            targetIndex++;
            index = (++index) % this._capacity;
            flag--;
        }
    }

    public Enumerator<T> GetEnumerator()
    {
        return new Enumerator<T>((LeakStack<T>)this);
    }

    public T Peek()
    {
        if (this._size == 0)
        {
            throw new InvalidOperationException("the stack is empty!");
        }
        return this._array[this._top];
    }

    public T Pop()
    {
        if (this._size == 0)
        {
            throw new InvalidOperationException();
        }
        this._version++;
        T local = this._array[this._top];
        this._array[this._top] = default(T);
        this._top = (--this._top + this._capacity) % this._capacity;
        this._size--;
        if (OnPoped != null)
            OnPoped(this, new EventArgs());
        return local;
    }

    /// <summary>
    /// This method will enlarge the capacity of the stack.
    /// </summary>
    /// <param name="item"></param>
    private void Add(T item)
    {
        if (this._size == this._array.Length)
        {
            this._capacity = (this._array.Length == 0) ? 4 : (2 * this._array.Length);
            T[] destinationArray = new T[this._capacity];
            Array.Copy(this._array, 0, destinationArray, 0, this._size);
            this._array = destinationArray;
        }
        this._top++;
        this._array[this._size++] = item;
        this._version++;
    }

    public void Push(T item)
    {
        this._top = (this._top + 1) % this._capacity;
        if (this._size == this._capacity)
        {
            if (OnLeak != null)
                OnLeak(this, new LeakEventArgs(this._array[this._top],this._capacity,this._top));
        }
        else
            this._size++;
        this._array[this._top] = item;
        this._version++;
        if (OnPushed != null)
            OnPushed(this, new EventArgs());
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return new Enumerator<T>((LeakStack<T>)this);
    }

    void ICollection.CopyTo(Array array, int arrayIndex)
    {
        if (array == null)
        {
            throw new ArgumentNullException();
        }
        if (array.Rank != 1)
        {
            throw new ArgumentException();
        }
        if (array.GetLowerBound(0) != 0)
        {
            throw new ArgumentException();
        }
        if ((arrayIndex < 0) || (arrayIndex > array.Length))
        {
            throw new ArgumentOutOfRangeException();
        }
        if ((array.Length - arrayIndex) < this._size)
        {
            throw new ArgumentException();
        }
        try
        {
            int index = ((this._top - this._size) + this._capacity) % this._capacity + 1;
            int flag = this._size;
            int targetIndex = arrayIndex;
            while (flag > 0)
            {
                array.SetValue(this._array[index], targetIndex);
                targetIndex++;
                index = (++index) % this._capacity;
                flag--;
            }
        }
        catch (ArrayTypeMismatchException)
        {
            throw new ArgumentException();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new Enumerator<T>((LeakStack<T>)this);
    }

    public T[] ToArray()
    {
        T[] localArray = new T[this._size];
        int index = this._top;
        for (int i = 0; i < this._size; i++)
        {
            localArray[i] = this._array[index];
            index = (--index + this._capacity) % this._capacity;
        }

        return localArray;
    }


    // Properties
    public int Count
    {
        get
        {
            return this._size;
        }
    }

    bool ICollection.IsSynchronized
    {
        get
        {
            return false;
        }
    }

    object ICollection.SyncRoot
    {
        get
        {
            if (this._syncRoot == null)
            {
                Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
            }
            return this._syncRoot;
        }
    }

    // Nested Types
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Enumerator<S> : IEnumerator<S>, IDisposable, IEnumerator
    {
        private LeakStack<S> _stack;
        private int _index;
        private int _version;
        private int _size;
        private int _accessed;
        private int _capacity;
        private S currentElement;

        internal Enumerator(LeakStack<S> stack)
        {
            this._stack = stack;
            this._version = this._stack._version;
            this._size = this._stack._size;
            this._index = this._stack._top;
            this._capacity = this._stack._capacity;
            this._accessed = 0;

            this.currentElement = default(S);
        }

        public void Dispose()
        {
            this._index = -1;
        }

        public bool MoveNext()
        {
            bool flag;
            if (this._version != this._stack._version)
            {
                throw new InvalidOperationException();
            }

            flag = this._accessed < this._size;
            if (flag)
            {
                this.currentElement = this._stack._array[this._index];
                this._index = (--this._index + this._capacity) % this._capacity;
                this._accessed++;
            }
            else
            {
                this.currentElement = default(S);
            }

            return flag;
        }

        public S Current
        {
            get
            {
                if (this._accessed == this._size)
                {
                    throw new InvalidOperationException();
                }
                return this.currentElement;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                if (this._accessed == this._size)
                {
                    throw new InvalidOperationException();
                }
                return this.currentElement;
            }
        }

        void IEnumerator.Reset()
        {
            if (this._version != this._stack._version)
            {
                throw new InvalidOperationException();
            }
            this._index = this._stack._top;
            this._accessed = 0;
            this.currentElement = default(S);
        }
    }

}