using System;
using System.Collections.Generic;
using System.Linq;

namespace TextGenerationWpf
{
    public static class Enumerable2
    {
        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource obj)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            yield return obj;

            foreach (var item in source)
                yield return item;
        }

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource obj)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var item in source)
                yield return item;

            yield return obj;
        }

        public static bool StartsWith<TSource>(this IList<TSource> source, IList<TSource> subsequence)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (subsequence == null) throw new ArgumentNullException(nameof(subsequence));

            return source.Count >= subsequence.Count &&
                source.Take(subsequence.Count).SequenceEqual(subsequence);
        }
    }
}
