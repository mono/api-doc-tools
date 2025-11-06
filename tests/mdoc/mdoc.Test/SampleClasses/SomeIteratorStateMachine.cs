using System;
using System.Collections;
using System.Collections.Generic;

namespace mdoc.Test.SampleClasses
{
    public class SomeIteratorStateMachine<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            yield return new KeyValuePair<TKey, TValue>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<T1, T2>> WithParameterType<T1, T2>()
        {
            yield return new KeyValuePair<T1, T2>();

        }

        public class SomeNestedIteratorStateMachine<NestedTKey, NestedTValue> : IEnumerable<KeyValuePair<NestedTKey, NestedTValue>>
        {
            public IEnumerator<KeyValuePair<NestedTKey, NestedTValue>> GetEnumerator()
            {
                yield return new KeyValuePair<NestedTKey, NestedTValue>();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public IEnumerator<KeyValuePair<NestedT1, NestedT2>> WithParameterType<NestedT1, NestedT2>()
            {
                yield return new KeyValuePair<NestedT1, NestedT2>();

            }
        }
    }
}
