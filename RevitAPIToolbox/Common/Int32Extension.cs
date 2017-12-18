using System;
using System.Collections.Generic;

namespace Techyard.Revit.Common
{
    public static class Int32Extension
    {
        public static void TraverseFrom(this int number, int from, Action<int> handler)
        {
            var value = from;
            while (value < number)
                handler?.Invoke(value++);
        }

        public static IEnumerable<T> TraverseFrom<T>(this int number, int from, Func<int, T> handler) where T : class
        {
            var value = from;
            while (value <= number)
                yield return handler?.Invoke(value++);
        }
    }
}