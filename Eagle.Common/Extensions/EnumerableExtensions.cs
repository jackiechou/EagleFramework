using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Eagle.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Contract.Requires(enumerable != null);
            Contract.Requires(action != null);

            var forEach = enumerable as T[] ?? enumerable.ToArray();
            foreach (var item in forEach)
            {
                action(item);
            }

            return forEach;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Func<T, bool> action)
        {
            Contract.Requires(enumerable != null);
            Contract.Requires(action != null);

            var forEach = enumerable as T[] ?? enumerable.ToArray();
            foreach (var item in forEach)
            {
                if (!action(item))
                {
                    break;
                }
            }

            return forEach;
        }

        public static IEnumerable<IEnumerable<T>> GroupsOf<T>(this IEnumerable<T> enumerable, int groupSize)
        {
            IList<T> group = new List<T>();

            foreach (T item in enumerable)
            {
                if (group.Count < groupSize)
                {
                    group.Add(item);
                }

                if (group.Count == groupSize)
                {
                    yield return group;

                    group = new List<T>();
                }
            }

            if (group.Count > 0)
            {
                yield return group;
            }
        }
    }
}
