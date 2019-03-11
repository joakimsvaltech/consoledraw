using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var x in list)
                action(x);
        }

        public static IEnumerable<T> Interleave<T>(this IEnumerable<T> items, T separator)
        {
            var list = items.ToArray();
            if (!list.Any())
                yield break;
            yield return list.First();
            foreach (var item in list.Skip(1))
            {
                yield return separator;
                yield return item;
            }
        }

        public static IEnumerable<T> Even<T>(this IEnumerable<T> items)
            => items.Select((item, i) => (item, i)).Where(t => t.i % 2 == 0).Select(t => t.item);

        public static IEnumerable<T> Odd<T>(this IEnumerable<T> items)
            => items.Select((item, i) => (item, i)).Where(t => t.i % 2 == 1).Select(t => t.item);

        public static IEnumerable<TRes> Combine<TItem, TRes>(
            this IEnumerable<TItem> items, 
            IEnumerable<TItem> otherItems, 
            Func<TItem, TItem, TRes> merge)
            => items.Select((item, i) => (item, i))
            .Join(
                otherItems.Select((item, i) => (item, i)), 
                outer => outer.i, 
                inner => inner.i, 
                (o, i) => merge(o.item, i.item));
    }
}
