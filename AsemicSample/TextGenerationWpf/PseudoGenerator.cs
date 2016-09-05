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

        List<TSource> Source;
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
            Source = new List<TSource>(source);
            Source.Insert(0, Delimiter);
            Source.Add(Delimiter);

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
            throw new NotImplementedException();
        }
    }
}
