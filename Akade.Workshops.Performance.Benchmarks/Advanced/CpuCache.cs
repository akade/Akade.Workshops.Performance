using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace Akade.Workshops.Performance.Benchmarks.Advanced;

/// <summary>
/// There are lot of things going on in this benchmark. Can you make it faster & explain why?
/// You are explicitly allowed to change the way the data is stored.
/// Hints:
/// - Q29udGlub3VzIG1lbW9yeSBhY2Nlc3MgaXMgZmFzdGVyIHRoYW4gcmFuZG9tIChsZXNzIGNhY2hlIG1pc3Nlcyk=
/// - TXVsdGlkaW1lbnNpb25hbCBhcnJheXMgYXJlIHNsb3dlciB0aGFuIGphZ2dlZCBvbmVzLCBhcyB0aGV5IGFsbG93IG1vcmUgdGhpbmdzLiBGb3IgZXhhbXBsZSwgdGhleSBhbGxvdyBuZWdhdGl2ZSBpbmRpY2VzIQ==
/// - SmFnZ2VkIGFycmF5cyBhcmUgc2xvd2VyIHRoYW4gYSBzaW5nbGUgY29udGlub3VzIGFycmF5
/// </summary>
[HardwareCounters(HardwareCounter.CacheMisses)]
[FastJob]
public class CpuCache
{
    private const int N = 1000;
    public int[,] _values;

    public CpuCache()
    {
        Random r = new(42);
        _values = new int[N, N];
        for (int x = 0; x < N; x++)
        {
            for (int y = 0; y < N; y++)
            {
                _values[x, y] = r.Next();

            }
        }
    }

    [Benchmark]
    public long Sum()
    {
        long sum = 0;

        for (int x = 0; x < N; x++)
        {
            for (int y = 0; y < N; y++)
            {
                sum += _values[y, x];
            }
        }

        return sum;
    }
}
