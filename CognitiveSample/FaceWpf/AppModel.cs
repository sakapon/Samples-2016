using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public ReactiveProperty<Face[]> DetectionResult { get; } = new ReactiveProperty<Face[]>();

        public AppModel()
        {
            ImagePath.Subscribe(_ => DetectAsync());
        }

        public async void DetectAsync()
        {
            if (string.IsNullOrWhiteSpace(ImagePath.Value)) return;

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
