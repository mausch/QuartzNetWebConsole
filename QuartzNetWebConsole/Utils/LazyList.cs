using System.Collections;
using System.Collections.Generic;

namespace QuartzNetWebConsole.Utils {
    public class LazyList<T> : IEnumerable<T> {
        private readonly T head;
        private readonly IEnumerable<T> tail;
        private readonly bool empty;

        public static readonly LazyList<T> Empty = new LazyList<T>();

        private LazyList() {
            empty = true;
        }

        public LazyList(T head, IEnumerable<T> tail) {
            this.head = head;
            this.tail = tail;
        }

        public IEnumerator<T> GetEnumerator() {
            if (empty)
                yield break;
            yield return head;
            foreach (var i in tail)
                yield return i;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}