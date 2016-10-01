using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TaskWebJob
{
    public static class Functions
    {
        [NoAutomaticTrigger]
        public static void RecordTime(
            DateTime startTime,
            IBinder binder,
            TextWriter logger)
        {
            var dt = $"{startTime:yyyyMMdd-HHmmss}";

            var blobAttribute = new BlobAttribute($"output/{dt}");
            var blob = binder.Bind<CloudBlockBlob>(blobAttribute);
            blob.UploadText(dt);

            logger.WriteLine(dt);
        }

        [NoAutomaticTrigger]
        public static void RecordTimeAndSleep(
            DateTime startTime,
            IBinder binder,
            TextWriter logger)
        {
            var start = $"{startTime:yyyyMMdd-HHmmss}";

            var blobAttribute = new BlobAttribute($"output/{start}");
            var blob = binder.Bind<CloudBlockBlob>(blobAttribute);

            blob.UploadText(start);
            logger.WriteLine(start);

            while (true)
            {
                Thread.Sleep(20 * 1000);

                var now = $"{DateTime.UtcNow:yyyyMMdd-HHmmss}";
                logger.WriteLine(now);
            }
        }

        [NoAutomaticTrigger]
        public static void RecordTimes(
            DateTime startTime,
            IBinder binder,
            TextWriter logger)
        {
            var start = $"{startTime:yyyyMMdd-HHmmss}";

            var blobAttribute = new BlobAttribute($"output/{start}");
            var blob = binder.Bind<CloudBlockBlob>(blobAttribute);

            logger.WriteLine(start);

            while (true)
            {
                Thread.Sleep(20 * 1000);

                var now = $"{DateTime.UtcNow:yyyyMMdd-HHmmss}";
                blob.UploadText($"{start}\r\n{now}");

                logger.WriteLine(now);
            }
        }
    }
}
