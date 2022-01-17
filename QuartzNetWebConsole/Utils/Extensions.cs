using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzNetWebConsole.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Map each element of a structure to a Task, evaluate these tasks from left to right, and collect the results.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Task<IReadOnlyList<TResult>> Traverse<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> f) {
            return source.Select(f).Sequence();
        }
        
        /// <summary>
        /// Evaluate each Task from left to right and collect the results.
        /// https://msdn.microsoft.com/en-us/library/hh194766.aspx
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static async Task<IReadOnlyList<TResult>> Sequence<TResult>(this IEnumerable<Task<TResult>> tasks)
        {
            var results = new List<TResult>();
            foreach (var t in tasks)
            {
                results.Add(await t);
            }
            return results;
        }
    }
}