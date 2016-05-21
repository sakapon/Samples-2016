using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace SensorUwp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs _e)
        {
            var lightSensor = LightSensor.GetDefault();
            lightSensor.ReportInterval = 500;
            Action<LightSensorReading> notifyLight =
                r => LightText.Text = $"Light: {r.IlluminanceInLux} lx";
            notifyLight(lightSensor.GetCurrentReading());
            lightSensor.ReadingChanged += (o, e) => RunOnUIAsync(() => notifyLight(e.Reading));

            var compass = Compass.GetDefault();
            compass.ReportInterval = 500;
            Action<CompassReading> notifyCompass =
                r => CompassText.Text = $"Compass: {r.HeadingMagneticNorth:N3} °";
            notifyCompass(compass.GetCurrentReading());
            compass.ReadingChanged += (o, e) => RunOnUIAsync(() => notifyCompass(e.Reading));
        }

        void RunOnUIAsync(DispatchedHandler action) =>
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
    }
}
