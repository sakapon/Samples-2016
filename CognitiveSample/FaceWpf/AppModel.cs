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

        public ReactiveProperty<string> ImageUrl { get; } = new ReactiveProperty<string>(mode: ReactivePropertyMode.None);
        public ReactiveProperty<BitmapImage> BitmapImage { get; } = new ReactiveProperty<BitmapImage>();
        public ReactiveProperty<Face[]> DetectionResult { get; } = new ReactiveProperty<Face[]>();

        public ReadOnlyReactiveProperty<bool> IsImageEmpty { get; }

        public AppModel()
        {
            // JPEG ファイルは DPI が異なる場合があります (既定では 96 だが、72 などもある)。
            // Image コントロールに直接読み込ませると、DPI によりサイズが変化してしまいます。
            ImageUrl
                .Subscribe(u =>
                {
                    var image = new BitmapImage(new Uri(u));

                    // ダウンロードの要否で分岐します。
                    if (image.IsDownloading)
                        image.DownloadCompleted += (o, e) => BitmapImage.Value = image;
                    else
                        BitmapImage.Value = image;
                });
            ImageUrl
                .Subscribe(_ => DetectAsync());

            IsImageEmpty = ImageUrl
                .Select(u => u == null)
                .ToReadOnlyReactiveProperty(true);
        }

        public async void DetectAsync()
        {
            if (string.IsNullOrWhiteSpace(ImageUrl.Value)) return;

            DetectionResult.Value = null;

            try
            {
                if (ImageUrl.Value.StartsWith("http://") || ImageUrl.Value.StartsWith("https://"))
                {
                    DetectionResult.Value = await Client.DetectAsync(ImageUrl.Value, returnFaceAttributes: (FaceAttributeType[])Enum.GetValues(typeof(FaceAttributeType)));
                }
                else
                {
                    using (var stream = File.OpenRead(ImageUrl.Value))
                    {
                        DetectionResult.Value = await Client.DetectAsync(stream, returnFaceAttributes: (FaceAttributeType[])Enum.GetValues(typeof(FaceAttributeType)));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
