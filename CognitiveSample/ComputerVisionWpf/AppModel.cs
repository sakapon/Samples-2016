using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Reactive.Bindings;

namespace ComputerVisionWpf
{
    public class AppModel
    {
        static string SubscriptionKey { get; } = ConfigurationManager.AppSettings["SubscriptionKey"];
        VisionServiceClient Client { get; } = new VisionServiceClient(SubscriptionKey);

        const string DefaultImageUrl = "https://model.foto.ne.jp/free/img/images_big/m010078.jpg";

        public ReactiveProperty<string> ImageUrl { get; } = new ReactiveProperty<string>(DefaultImageUrl);
        public ReactiveProperty<AnalysisResult> AnalysisResult { get; } = new ReactiveProperty<AnalysisResult>();
        public ReactiveProperty<OcrResults> OcrResults { get; } = new ReactiveProperty<OcrResults>();

        public async void Analyze()
        {
            if (string.IsNullOrWhiteSpace(ImageUrl.Value)) return;

            AnalysisResult.Value = null;
            OcrResults.Value = null;

            try
            {
                AnalysisResult.Value = await Client.AnalyzeImageAsync(ImageUrl.Value, (VisualFeature[])Enum.GetValues(typeof(VisualFeature)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            try
            {
                OcrResults.Value = await Client.RecognizeTextAsync(ImageUrl.Value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
