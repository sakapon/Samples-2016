using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Reactive.Bindings;

namespace ComputerVisionWpf
{
    public class AppModel
    {
        static readonly string SubscriptionKey = ConfigurationManager.AppSettings["SubscriptionKey"];

        VisionServiceClient Client { get; } = new VisionServiceClient(SubscriptionKey);

        public ReactiveProperty<string> ImageUrl { get; } = new ReactiveProperty<string>("");
        public ReactiveProperty<AnalysisResult> Result { get; } = new ReactiveProperty<AnalysisResult>();

        public async void Analyze()
        {
            if (string.IsNullOrWhiteSpace(ImageUrl.Value)) return;

            Result.Value = await Client.AnalyzeImageAsync(ImageUrl.Value, (VisualFeature[])Enum.GetValues(typeof(VisualFeature)));
        }
    }
}
