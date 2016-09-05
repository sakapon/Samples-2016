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

        Dictionary<TKey, double> SubelementMap1;
        Dictionary<TKey, double> SubelementMap;

        public PseudoGenerator(TSource delimiter, Func<IEnumerable<TSource>, TKey> keySelector)
        {
            Delimiter = delimiter;
            KeySelector = keySelector;
        }

        public void Train(IEnumerable<TSource> source)
        {
            var elements = new List<TSource>(source);
            elements.Insert(0, Delimiter);
            elements.Add(Delimiter);

            SubelementMap1 = ToSubelementMap(elements, 1);
            SubelementMap = Enumerable.Range(2, MaxSubelementsLength - 1)
                .SelectMany(i => ToSubelementMap(elements, i))
                .ToDictionary(p => p.Key, p => p.Value);
        }

        Dictionary<TKey, double> ToSubelementMap(IList<TSource> elements, int subelementsLength) =>
            Enumerable.Range(0, TrialCount)
                .Select(_ => elements.GetRandomPiece(subelementsLength))
                .ToCountMap(KeySelector)
                .ToProbabilityMap()
                .ToEnhancedProbabilityMap(x => Math.Pow(x, FeatureWeight));

        public TSource[] Generate()
        {
            throw new NotImplementedException();
        }
    }
}
