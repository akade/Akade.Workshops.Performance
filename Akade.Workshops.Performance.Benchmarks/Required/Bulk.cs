using BenchmarkDotNet.Attributes;

namespace Akade.Workshops.Performance.Benchmarks.Required;

/// <summary>
/// This benchmark demonstrate how your interface may affect performance
/// Can you make it faster? You are allowed to change the interface & body of GET but you have to execute the Network latency
/// Hints:
/// - Q2FuIHlvdSBwYXkgdGhlIG5ldHdvcmsgbGF0ZW5jeSBvbmx5IG9uY2U/
/// - TW9kaWZ5IGdldCB0byB0YWtlIGEgbGlzdCBvZiBpZHMgYW5kIHJldHVybiBhbGwgcmVzdWx0cyBpbiBvbmUgbWV0aG9kIGNhbGw=
/// </summary>
[FastJob(unrollFactor: 1, maxIterationCount: 3)]
public class Bulk
{
    public int[] ids = Enumerable.Range(0, 100).ToArray();


    [Benchmark]
    public int Call()
    {
        List<bool> dtos = new();
        for (int i = 0; i < ids.Length; i++)
        {
            dtos.Add(Get(i));
        }
        return dtos.Count(x => x);
    }


    private bool Get(int id)
    {
        Thread.Sleep(5); // Network latency
        return id % 2 == 0;
    }

}
