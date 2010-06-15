using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace QuartzNetWebConsole.Utils {
    public class CircularBuffer<T> : IEnumerable<T> {
        private LazyList<T> list = LazyList<T>.Empty;
        private readonly int capacity;

        public CircularBuffer(int capacity) {
            this.capacity = capacity;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(T e) {
            list = new LazyList<T>(e, list.Take(capacity-1));
        }

        public IEnumerator<T> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}