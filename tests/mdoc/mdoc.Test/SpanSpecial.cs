using System;

namespace mdoc.Test
{
    public struct SpanSpecial<T>
    {
        public struct Enumerator
        {
            private readonly SpanSpecial<T> _span;

            private int _index;

            public T Current
            {
                get
                {
                    return this._span[this._index];
                }
            }

            internal Enumerator(SpanSpecial<T> span)
            {
                this._span = span;
                this._index = -1;
            }

            public bool MoveNext()
            {
                int num = this._index + 1;
                if (num < this._span.Length)
                {
                    this._index = num;
                    return true;
                }
                return false;
            }
        }

        private readonly int _length;

        public int Length
        {
            get
            {
                return this._length;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this._length == 0;
            }
        }

        public static SpanSpecial<T> Empty
        {
            get
            {
                return default(SpanSpecial<T>);
            }
        }

        public ref T this[int index]
        {
            get
            {
                if (index >= this._length)
                {
                    throw new Exception("error");
                }

                return  ref this[index];
      
            }
        }

        public SpanSpecial<T>.Enumerator GetEnumerator()
        {
            return new SpanSpecial<T>.Enumerator(this);
        }

        public SpanSpecial( int length)
        {
            this._length = length;
        }

    }
}
