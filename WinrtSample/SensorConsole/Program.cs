using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace SensorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var lightSensor = LightSensor.GetDefault();
            lightSensor.ReportInterval = 500;
            Action<LightSensorReading> notifyLight =
                r => Console.WriteLine($"Light: {r.IlluminanceInLux} lx");
            notifyLight(lightSensor.GetCurrentReading());
            lightSensor.ReadingChanged += (o, e) => notifyLight(e.Reading);

            var compass = Compass.GetDefault();
            compass.ReportInterval = 500;
            Action<CompassReading> notifyCompass =
                r => Console.WriteLine($"Compass: {r.HeadingMagneticNorth:N3} °");
            notifyCompass(compass.GetCurrentReading());
            compass.ReadingChanged += (o, e) => notifyCompass(e.Reading);

            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
