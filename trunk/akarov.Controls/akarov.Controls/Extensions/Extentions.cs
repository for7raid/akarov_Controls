using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq.Expressions;

namespace akarov.Controls.Extensions
{
    public static class Extentions
    {
        

        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o);
        }

        public static TResult Return<TInput, TResult>(this TInput o,
                                                        Func<TInput, TResult> evaluator,
                                                        TResult failureValue) where TInput : class
        {
            if (o == null) return failureValue;
            return evaluator(o);
        }

        public static IEnumerable<List<T>> Partition<T>(this IList<T> source, Int32 size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (Double)size); i++)
                yield return new List<T>(source.Skip(size * i).Take(size));
        }

        public static bool In<T>(this T source, params T[] list)
        {
            return list.Any(t => t.Equals(source));
        }

        public static bool In(this Enum elem, params Enum[] array)
        {
            foreach (object t in array)
                if (t.Equals(elem)) return true;
            return false;
        }


    }
}