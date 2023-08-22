
/* Unmerged change from project 'Akade.Workshops.Performance.Benchmarks (net7.0)'
Before:
using BenchmarkDotNet.Attributes;
After:
using Akade;
using Akade.Workshops;
using Akade.Workshops.Performance;
using Akade.Workshops.Performance.Benchmarks;
using Akade.Workshops.Performance.Benchmarks;
using Akade.Workshops.Performance.Benchmarks.DataStructures;
using BenchmarkDotNet.Attributes;
*/
using BenchmarkDotNet.Attributes;

namespace Akade.Workshops.Performance.Benchmarks.Required;

/// <summary>
/// This benchmark demonstrates how Linq is evaluated "on demand".
/// Can you make it faster?
/// Hints:
/// - Note: Do not change LoadDataFromServer
/// - V2hlbiBpcyBMb2FkRGF0YUZyb21TZXJ2ZXIgY2FsbGVkPw==
/// - RWFjaCB0aW1lIGRhdGFGcm9tU2VydmVyIGlzIHVzZWQgYmVsb3csIExvYWREYXRhRnJvbVNlcnZlciBpcyBjYWxsZWQgYWdhaW4gZHVlIHRvIHRoZSBsYXp5IG5hdHVyZS4=
/// - TWF0ZXJpYWxpemUgdGhlIGxpc3QgdXNpbmcgVG9BcnJheSgpIG9yIFRvTGlzdCgpLiBIb3cgb2Z0ZW4gaXMgTG9hZERhdGFGcm9tU2VydmVyKCkgbm93IGNhbGxlZD8=
/// - Qm9udXM6IFRha2UgYSBsb29rIGF0IHRoZSBydW50aW1lIGNvbXBsZXhpdHksIGNhbiB5b3UgbWFrZSB0aGF0IGJldHRlcj8=
/// </summary>
[FastJob(unrollFactor: 1)]
public class LazyLinqEnumeration
{
    [Benchmark]
    public double CalculateStandardDeviation()
    {
        var dataFromServer = LoadDataFromServer();
        // sqrt{sum[(x-x_avg)^2]/(n-1)}
        double sumOfSquaredDifferences = dataFromServer.Sum(x => Math.Pow(x - dataFromServer.Average(), 2));
        return Math.Sqrt(sumOfSquaredDifferences / (dataFromServer.Count() - 1));
    }

    private static IEnumerable<int> LoadDataFromServer()
    {
        Thread.Sleep(5); // costly remote operation
        for (int i = 0; i < 5; i++)
        {
            yield return i;
        }
    }
}