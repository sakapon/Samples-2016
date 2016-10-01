using System;
using System.Collections.Generic;
using System.IO;
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
                host.RunAndBlock();
            }
        }
    }

    public static class PrimeNumbersFunctions
    {
        public static void GetPrimeNumbers(
            [QueueTrigger("primenumbers")] PrimeNumbersArgs args,
            [Blob("primenumbers/{MinValue}-{MaxValue}", FileAccess.Write)] Stream outStream,
            TextWriter logger)
        {
            logger.WriteLine($"{DateTime.UtcNow:MM/dd HH:mm:ss.fff}: Begin");

            var result = PrimeNumbers.GetPrimeNumbers(args.MinValue, args.MaxValue);

            using (var writer = new StreamWriter(outStream))
            {
                foreach (var p in result)
                {
                    writer.WriteLine(p);
                }
            }

            logger.WriteLine($"{DateTime.UtcNow:MM/dd HH:mm:ss.fff}: End");
        }
    }

    public class PrimeNumbersArgs
    {
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
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
