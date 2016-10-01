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
                host.Call(typeof(Functions).GetMethod(nameof(Functions.RecordTimeAndSleep)), new { startTime = DateTime.UtcNow });
            }
        }
    }

    public static class PrimeNumbers
    {
        public static IEnumerable<long> GetPrimeNumbers(long minValue, long maxValue) =>
            new[]
            {
                new
                {
                    primes = new List<long>(),
                    min = Math.Max(minValue, 2),
                    max = Math.Max(maxValue, 0),
                    root_max = maxValue >= 0 ? (long)Math.Sqrt(maxValue) : 0,
                }
            }
                .SelectMany(_ => Enumerable2.Range2(2, Math.Min(_.root_max, _.min - 1))
                    .Concat(Enumerable2.Range2(_.min, _.max))
                    .Select(i => new { _.primes, i, root_i = (long)Math.Sqrt(i) }))
                .Where(_ => _.primes
                    .TakeWhile(p => p <= _.root_i)
                    .All(p => _.i % p != 0))
                .Do(_ => _.primes.Add(_.i))
                .Select(_ => _.i)
                .SkipWhile(i => i < minValue);
    }

    public static class Enumerable2
    {
        public static IEnumerable<long> Range2(long minValue, long maxValue)
        {
            for (var i = minValue; i <= maxValue; i++)
            {
                yield return i;
            }
        }

        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }
    }
}
