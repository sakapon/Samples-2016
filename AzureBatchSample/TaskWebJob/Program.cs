using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;

namespace TaskWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            using (var host = new JobHost())
            {
                host.Call(typeof(Functions).GetMethod(nameof(Functions.RecordTimes)), new { startTime = DateTime.UtcNow });
            }
        }
    }
}
