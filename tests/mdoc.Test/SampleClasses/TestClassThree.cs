using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdoc.Test.SampleClasses
{
    public class TestClassThree : IEnumerable<KeyValuePair<string, TestClassTwo>>, IDictionary<string, TestClassTwo>, ICollection<KeyValuePair<string, TestClassTwo>>, IEnumerable
    {
        private IDictionary<string, TestClassTwo> _dictionary;

        public TestClassThree()
        {
            this._dictionary = new Dictionary<string, TestClassTwo>();
        }

        public TestClassTwo this[string key] { get => this._dictionary[key]; set => new NotImplementedException(); }

        int ICollection<KeyValuePair<string, TestClassTwo>>.Count => this._dictionary.Count;

        bool ICollection<KeyValuePair<string, TestClassTwo>>.IsReadOnly => throw new NotImplementedException();

        ICollection<string> IDictionary<string, TestClassTwo>.Keys => this._dictionary.Keys;

        ICollection<TestClassTwo> IDictionary<string, TestClassTwo>.Values => throw new NotImplementedException();

        void ICollection<KeyValuePair<string, TestClassTwo>>.Add(KeyValuePair<string, TestClassTwo> item)
        {
            throw new NotImplementedException();
        }

        void IDictionary<string, TestClassTwo>.Add(string key, TestClassTwo value)
        {
            _dictionary[key] = value;
        }

        void ICollection<KeyValuePair<string, TestClassTwo>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, TestClassTwo>>.Contains(KeyValuePair<string, TestClassTwo> item)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, TestClassTwo>.ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, TestClassTwo>>.CopyTo(KeyValuePair<string, TestClassTwo>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, TestClassTwo>>)this._dictionary).CopyTo(array, arrayIndex);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, TestClassTwo>> IEnumerable<KeyValuePair<string, TestClassTwo>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, TestClassTwo>>.Remove(KeyValuePair<string, TestClassTwo> item)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, TestClassTwo>.Remove(string key)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, TestClassTwo>.TryGetValue(string key, out TestClassTwo value)
        {
            throw new NotImplementedException();
        }
    }
}
