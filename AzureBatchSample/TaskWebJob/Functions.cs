using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
