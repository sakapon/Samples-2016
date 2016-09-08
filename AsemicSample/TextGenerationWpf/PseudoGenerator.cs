using System;
using System.Collections.Generic;
using System.Linq;

namespace TextGenerationWpf
{
    public class PseudoGenerator<TSource, TKey>
    {
        public int MaxSubelementsLength { get; set; } = 4;
        public int TrialCount { get; set; } = 10000;
        public double FeatureWeight { get; set; } = 2.0;

        public TSource Delimiter { get; }
        public Func<IEnumerable<TSource>, TKey> KeySelector { get; }

        TSource[] Source;
        Dictionary<TKey, TSource[]> Subelements = new Dictionary<TKey, TSource[]>();
        Dictionary<TKey, double> SubelementMap1;
        Dictionary<TKey, double> SubelementMap;

        public PseudoGenerator(TSource delimiter, Func<IEnumerable<TSource>, TKey> keySelector)
        {
            Delimiter = delimiter;
            KeySelector = keySelector;
        }

        public void Train(IEnumerable<TSource> source)
        {
            Source = source
                .Prepend(Delimiter)
                .Append(Delimiter)
                .ToArray();

            SubelementMap1 = ToSubelementMap(Source, 1);
            SubelementMap = Enumerable.Range(2, MaxSubelementsLength - 1)
                .SelectMany(i => ToSubelementMap(Source, i))
                .ToDictionary(p => p.Key, p => p.Value);
        }

        Dictionary<TKey, double> ToSubelementMap(IList<TSource> elements, int subelementsLength)
        {
            var subelements = Enumerable.Range(0, TrialCount)
                .Select(_ => elements.GetRandomPiece(subelementsLength).ToArray())
                .Select(se => new { se, key = KeySelector(se) })
                .ToArray();

            foreach (var _ in subelements)
                Subelements[_.key] = _.se;

            return subelements
                .ToCountMap(_ => _.key)
                .ToProbabilityMap()
                .ToEnhancedProbabilityMap(x => Math.Pow(x, FeatureWeight));
        }

        public TSource[] Generate()
        {
            var newElements = new List<TSource> { Delimiter };
            while (true)
            {
                newElements.AddRange(CreateNextElements(newElements));

                var availableLength = newElements.LastIndexOf(Delimiter) - 1;
                if (availableLength >= Source.Length)
                    return newElements.Skip(1).Take(availableLength).ToArray();
            }
        }

        TSource[] CreateNextElements(List<TSource> elements)
        {
            var probabilityMap = Enumerable.Range(1, MaxSubelementsLength - 1)
                .TakeWhile(i => i <= elements.Count)
                .Select(l => elements.GetRange(elements.Count - l, l))
                .SelectMany(last => SubelementMap
                    .Where(p => Subelements[p.Key].Length > last.Count && Subelements[p.Key].StartsWith(last))
                    .Select(p => CreatePair(new { last, subelement = Subelements[p.Key] }, p.Value)))
                .ToDictionary(p => p.Key, p => p.Value)
                .ToProbabilityMap();

            if (probabilityMap.Count == 0)
                return Subelements[SubelementMap1.GetNextRandomElement()];

            var joint = probabilityMap.GetNextRandomElement();
            return joint.subelement.Skip(joint.last.Count).ToArray();
        }

        static KeyValuePair<TKey_, TValue> CreatePair<TKey_, TValue>(TKey_ key, TValue value) => new KeyValuePair<TKey_, TValue>(key, value);
    }
}
