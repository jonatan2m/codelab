using System;
using System.Collections.Generic;
using System.Linq;

namespace Refactoring.KeepingFocusOnDomainWithSequences
{
    public static class EnumerableExtension
    {
        public static T WithMinimum<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> criterion)
        where T : class
        where TKey : IComparable<TKey>
        {
            return sequence
                .Aggregate((T)null, (best, cur) =>
                    best == null || criterion(cur).CompareTo(criterion(best)) < 0 ? cur : best);

        }

        public static T WithMinimumBetter<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> criterion)
            where T : class
            where TKey : IComparable<TKey>
        {
            var result = sequence
                .Select(x => Tuple.Create(x, criterion(x)))
                .Aggregate((Tuple<T, TKey>) null,
                    (best, cur) => best == null || cur.Item2.CompareTo(best.Item2) < 0 ? cur : best);

            return result == null ? default(T) : result.Item1;
        }
    }
}
