using System.Collections.Generic;

namespace QuartzNetWebConsole {
    internal static class EnumerableExtensions {
        public static HashSet<T> ToSet<T>(this IEnumerable<T> l) {
            return new HashSet<T>(l);
        }
    }
}