using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Reactive.Bindings;

namespace FaceWpf
{
    public class AppModel
    {
        // Microsoft Cognitive Services - Face API
        // https://www.microsoft.com/cognitive-services/en-us/face-api

        static string SubscriptionKey { get; } = ConfigurationManager.AppSettings["SubscriptionKey"];
        FaceServiceClient Client { get; } = new FaceServiceClient(SubscriptionKey);

        public ReactiveProperty<string> ImagePath { get; } = new ReactiveProperty<string>(mode: ReactivePropertyMode.None);
        public ReadOnlyReactiveProperty<BitmapImage> BitmapImage { get; }
        public ReactiveProperty<Face[]> DetectionResult { get; } = new ReactiveProperty<Face[]>();

        public AppModel()
        {
            // JPEG ファイルは DPI が異なる場合があります (既定では 96 だが、72 などもある)。
            // Image コントロールに直接読み込ませると、DPI によりサイズが変化してしまいます。
            BitmapImage = ImagePath
                .Select(p=> new BitmapImage(new Uri(p)))
                .ToReadOnlyReactiveProperty();
            ImagePath.Subscribe(_ => DetectAsync());
        }

        public async void DetectAsync()
        {
            if (string.IsNullOrWhiteSpace(ImagePath.Value)) return;

            DetectionResult.Value = null;

            try
            {
                using (var stream = File.OpenRead(ImagePath.Value))
                {
                    DetectionResult.Value = await Client.DetectAsync(stream, returnFaceAttributes: (FaceAttributeType[])Enum.GetValues(typeof(FaceAttributeType)));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
