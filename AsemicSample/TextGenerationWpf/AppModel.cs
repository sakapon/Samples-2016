using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;

namespace TextGenerationWpf
{
    public class AppModel
    {
        const string Arinomamade_Lyrics = @"
ふりはじめたゆきは
あしあとけして
まっしろなせかいに
ひとりのわたし
かぜがこころにささやくの
このままじゃだめなんだと
とまどいきずつき
だれにもうちあけずに
なやんでた
それももうやめよう
ありのままの
すがたみせるのよ
ありのままの
じぶんになるの
なにもこわくない
かぜよふけ
すこしもさむくないわ
";

        public ReactiveProperty<double> FeatureWeight { get; } = new ReactiveProperty<double>(2.0);
        public ReactiveProperty<string> SourceText { get; } = new ReactiveProperty<string>(Arinomamade_Lyrics);
        public ReactiveProperty<string> GeneratedText { get; } = new ReactiveProperty<string>("");

        public void GenerateText()
        {
            var generator = new OneTextGenerator { Delimiter = '\n' };
            GeneratedText.Value = generator.Generate(SourceText.Value.Replace("\r\n", "\n"));
        }
    }
}
