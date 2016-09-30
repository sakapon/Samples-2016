using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.WindowsAzure.Storage;

namespace BatchTaskConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var accountString = ConfigurationManager.ConnectionStrings["StorageAccount"].ConnectionString;
            var account = CloudStorageAccount.Parse(accountString);

            var blobClient = account.CreateCloudBlobClient();
            var container_output = blobClient.GetContainerReference("output");

            var dt = $"{DateTime.UtcNow:yyyyMMdd-HHmmss}";
            var blob = container_output.GetBlockBlobReference(dt);
            blob.UploadText(dt);
        }
    }
}
