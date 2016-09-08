using System;
using System.Collections.Generic;
using System.Linq;

namespace TextGenerationWpf
{
    public static class RandomHelper
    {
        static readonly Random Random = new Random();

        // The sum of values must be 1.
        public static T GetNextRandomElement<T>(this Dictionary<T, double> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var v = Random.NextDouble();

            var sum = 0.0;
            foreach (var p in source)
            {
                sum += p.Value;

                if (sum > v) return p.Key;
            }

            return source.FirstOrDefault().Key;
        }

        public static IEnumerable<T> GetRandomPiece<T>(this IList<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (count < 0) throw new ArgumentOutOfRangeException("count", count, "The value must be non-negative.");

            if (count == 0) return Enumerable.Empty<T>();
            if (count >= source.Count) return source;

            return Enumerable.Range(Random.Next(source.Count - count + 1), count)
                .Select(i => source[i]);
        }

        public static Dictionary<TKey, int> ToCountMap<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return source
                .GroupBy(keySelector)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public static Dictionary<T, double> ToProbabilityMap<T>(this Dictionary<T, int> countMap)
        {
            if (countMap == null) throw new ArgumentNullException("countMap");

            var countSum = countMap.Values.Sum();
            if (countSum == 0) return new Dictionary<T, double>();

            return countMap.ToDictionary(p => p.Key, p => (double)p.Value / countSum);
        }

        public static Dictionary<T, double> ToProbabilityMap<T>(this Dictionary<T, double> countMap)
        {
            if (countMap == null) throw new ArgumentNullException("countMap");

            var countSum = countMap.Values.Sum();
            if (countSum == 0.0) return new Dictionary<T, double>();

            return countMap.ToDictionary(p => p.Key, p => p.Value / countSum);
        }

        public static Dictionary<T, double> ToEnhancedProbabilityMap<T>(this Dictionary<T, double> countMap, Func<double, double> enhanceValue)
        {
            if (countMap == null) throw new ArgumentNullException("countMap");
            if (enhanceValue == null) throw new ArgumentNullException("enhanceValue");

            return countMap
                .ToDictionary(p => p.Key, p => enhanceValue(p.Value))
                .ToProbabilityMap();
        }
    }
}
