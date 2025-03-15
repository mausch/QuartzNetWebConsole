using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QuartzNetWebConsole.Utils {
    /// <summary>
    /// List with fixed capacity, FIFO removal
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitedList<T> : IEnumerable<T> {
        private readonly LinkedList<T> list = new LinkedList<T>();
        private readonly int capacity;

        public LimitedList(int capacity) {
            this.capacity = capacity;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(T e) {
            list.AddFirst(e);
            if (list.Count > capacity)
                list.RemoveLast();
        }

        public IEnumerator<T> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}