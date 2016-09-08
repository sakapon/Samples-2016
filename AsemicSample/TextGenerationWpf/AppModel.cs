using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;

namespace TextGenerationWpf
{
    public class AppModel
    {
        public ReactiveProperty<int> MaxSubelementsLength { get; } = new ReactiveProperty<int>(4);
        public ReactiveProperty<double> FeatureWeight { get; } = new ReactiveProperty<double>(2.0);

        public ReactiveProperty<string> SourceText { get; } = new ReactiveProperty<string>(Lyrics_Arinomamade);
        public ReactiveProperty<string> GeneratedText { get; } = new ReactiveProperty<string>("");

        public void GenerateText()
        {
            var generator = new PseudoGenerator<char, string>('\n', ToString)
            {
                MaxSubelementsLength = MaxSubelementsLength.Value,
                FeatureWeight = FeatureWeight.Value,
            };
            generator.Train(SourceText.Value.Replace("\r\n", "\n"));
            GeneratedText.Value = ToString(generator.Generate());
        }

        static string ToString(IEnumerable<char> chars) => new string(chars.ToArray());

        const string Lyrics_Arinomamade = @"ふりはじめたゆきは
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
すこしもさむくないわ";

        const string Lyrics_LetItGo = @"The snow glows white on the mountain tonight
Not a footprint to be seen.
A kingdom of isolation,
and it looks like I'm the Queen
The wind is howling like this swirling storm inside
Couldn't keep it in;
Heaven knows I've tried
Don't let them in,
don't let them see
Be the good girl you always have to be
Conceal, don't feel,
don't let them know
Well now they know
Let it go, let it go
Can't hold it back anymore
Let it go, let it go
Turn away and slam the door
I don't care
what they're going to say
Let the storm rage on.
The cold never bothered me anyway";
    }
}
