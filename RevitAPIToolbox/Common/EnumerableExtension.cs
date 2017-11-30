using System;
using System.Collections;
using System.Collections.Generic;

namespace Techyard.Revit.Common
{
    public static class EnumerableExtension
    {
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable enumerable,
            Func<TSource, TResult> selector)
            where TSource : class
            where TResult : class
        {
            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = (TSource) enumerator.Current;
                yield return selector?.Invoke(item);
            }
        }

        public static IEnumerable<T> AsList<T>(this IEnumerable enumerable)
            where T : class
        {
            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = (T) enumerator.Current;
                yield return item;
            }
        }
    }
}
