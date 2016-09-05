using System;
using System.Collections.Generic;
using System.Linq;

namespace TextGenerationWpf
{
    public class OneTextGenerator
    {
        public int MaxSubstringLength { get; set; } = 4;
        public int TrialCount { get; set; } = 10000;
        public char Delimiter { get; set; } = ' ';
        public double FeatureWeight { get; set; } = 2.0;

        Element<char, int> Separator;
        Dictionary<string, double> SubstringMap1;
        Dictionary<string, double> SubstringMap;

        public string Generate(string text)
        {
            Separator = new Element<char, int> { Source = Delimiter, ExtraData = -1 };
            var textWithIndex = text.Split(Delimiter)
                .Select(AddIndex)
                .SequenceJoin(Separator)
                .ToArray();

            SubstringMap1 = textWithIndex.ToSubelementMap(ToString, 1, TrialCount, FeatureWeight);
            SubstringMap = Enumerable.Range(2, MaxSubstringLength - 1)
                .Select(i => textWithIndex.ToSubelementMap(ToString, i, TrialCount, FeatureWeight))
                .SelectMany(d => d)
                .ToDictionary(p => p.Key, p => p.Value);

            var newChars = new List<char> { Delimiter };
            while (true)
            {
                newChars.AddRange(CreateNextChars(newChars));

                var availableLength = newChars.LastIndexOf(Delimiter) + 1;
                if (availableLength >= text.Length)
                    return ToString(newChars.Take(availableLength));
            }
        }

        static IEnumerable<Element<char, int>> AddIndex(string text) =>
            text.Select((c, i) => new Element<char, int> { Source = c, ExtraData = i });

        static string ToString(IEnumerable<char> chars) => string.Join("", chars);

        char[] CreateNextChars(List<char> chars)
        {
            // TODO: 短すぎ・長すぎを抑制する処理。
            if (chars.Count - chars.LastIndexOf(Delimiter) > 15)
                return new[] { Delimiter };

            var probabilityMap = Enumerable.Range(1, MaxSubstringLength - 1)
                .TakeWhile(i => i <= chars.Count)
                .Select(l => chars.GetRange(chars.Count - l, l))
                .SelectMany(cs => SubstringMap
                    .Where(p => p.Key.Length > cs.Count && p.Key.StartsWith(ToString(cs)))
                    .Select(p => CreatePair(new { last = cs, substring = p.Key }, p.Value)))
                .ToDictionary(p => p.Key, p => p.Value)
                .ToProbabilityMap();

            if (probabilityMap.Count == 0)
                return SubstringMap1.GetNextRandomElement().ToCharArray();

            var next = probabilityMap.GetNextRandomElement();
            return next.substring.Substring(next.last.Count).ToCharArray();
        }

        static KeyValuePair<TKey, TValue> CreatePair<TKey, TValue>(TKey key, TValue value) => new KeyValuePair<TKey, TValue>(key, value);
    }

    public class Element<TSource, TData>
    {
        public TSource Source { get; set; }
        public TData ExtraData { get; set; }
    }

    public static class TextGenerationHelper
    {
        public static Dictionary<TKey, double> ToSubelementMap<TSource, TData, TKey>(this IList<Element<TSource, TData>> elements, Func<IEnumerable<TSource>, TKey> keySelector, int subelementsLength, int trialCount, double featureWeight)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            return Enumerable.Range(0, trialCount)
                .Select(_ => elements.GetRandomPiece(subelementsLength))
                .ToCountMap(es => keySelector(es.Select(e => e.Source)))
                .ToProbabilityMap()
                .ToEnhancedProbabilityMap(x => Math.Pow(x, featureWeight));
        }

        public static IEnumerable<TSource> SequenceJoin<TSource>(this IEnumerable<IEnumerable<TSource>> source, TSource separator)
        {
            if (source == null) throw new ArgumentNullException("source");

            var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
            {
                foreach (var item in enumerator.Current)
                    yield return item;
            }
            else
            {
                yield break;
            }

            while (enumerator.MoveNext())
            {
                yield return separator;

                foreach (var item in enumerator.Current)
                    yield return item;
            }
        }

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

        public static Dictionary<T, double> ToEnhancedProbabilityMap<T>(this Dictionary<T, double> countMap) => countMap.ToEnhancedProbabilityMap(x => x * x);

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
