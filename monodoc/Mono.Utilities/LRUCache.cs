using System;
using System.Collections.Generic;

namespace Mono.Utilities
{
    public class LRUCache<TKey, TValue>
    {
	    [ThreadStatic]
	    static LRUCache<TKey, TValue> deflt;

	    public static LRUCache<TKey, TValue> Default {
		    get {
			    return deflt != null ? deflt : (deflt = new LRUCache<TKey, TValue> (5));
		    }
	    }

        int capacity;
        LinkedList<ListValueEntry<TKey, TValue>> list;
        Dictionary<TKey, LinkedListNode<ListValueEntry<TKey, TValue>>> lookup;
        LinkedListNode<ListValueEntry<TKey, TValue>> openNode;

        public LRUCache (int capacity)
        {
            this.capacity = capacity;
            this.list = new LinkedList<ListValueEntry<TKey, TValue>>();
            this.lookup = new Dictionary<TKey, LinkedListNode<ListValueEntry<TKey, TValue>>> (capacity + 1);
            this.openNode = new LinkedListNode<ListValueEntry<TKey, TValue>>(new ListValueEntry<TKey, TValue> (default(TKey), default(TValue)));
        }

        public void Put (TKey key, TValue value)
        {
            if (Get(key) == null) {
                if (this.openNode.Value == null)
                    this.openNode.Value = new ListValueEntry<TKey, TValue>(default(TKey), default(TValue));

                this.openNode.Value.ItemKey = key;
                this.openNode.Value.ItemValue = value;
                this.list.AddFirst (this.openNode);
                this.lookup.Add (key, this.openNode);

                if (this.list.Count > this.capacity) {
                    // last node is to be removed and saved for the next addition to the cache
                    this.openNode = this.list.Last;

                    // remove from list & dictionary
                    this.list.RemoveLast();
                    this.lookup.Remove(this.openNode.Value.ItemKey);
                } else {
                    // still filling the cache, create a new open node for the next time
                    this.openNode = new LinkedListNode<ListValueEntry<TKey, TValue>>(new ListValueEntry<TKey, TValue>(default(TKey), default(TValue)));
                }
            }
        }

        public TValue Get (TKey key)
        {
            LinkedListNode<ListValueEntry<TKey, TValue>> node = null;
            if (!this.lookup.TryGetValue (key, out node))
                return default (TValue);
            this.list.Remove (node);
            this.list.AddFirst (node);
            return node.Value.ItemValue;
        }

        class ListValueEntry<K, V> where K : TKey 
                                   where V : TValue
        {
            internal V ItemValue;
            internal K ItemKey;

            internal ListValueEntry(K key, V value)
            {
                this.ItemKey = key;
                this.ItemValue = value;
            }
        }
    }
}
