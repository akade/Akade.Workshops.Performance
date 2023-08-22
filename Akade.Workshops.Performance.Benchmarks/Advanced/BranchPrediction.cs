using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace Akade.Workshops.Performance.Benchmarks.Advanced;

/// <summary>
/// The following benchmark allows you to think about superscalar execution. Can you make it faster?
/// Hints:
/// - TW9kZXJuIENQVXMgc3BlY3VsYXRpdmx5IGV4ZWN1dGUgYnJhbmNoZWQgY29kZSwgaWYgdGhleSBnZXQgaXQgd3JvbmcsIGl0IGlzIGV4cGVuc2l2ZS4=
/// - VHJ5IHRvIHJld3JpdGUgbG9vcCB3aXRob3V0IHRoZSBpZiBpbiBpdA==
/// - Qml0IG9wZXJhdGlvbnMgc3VjaCBhcyBzaGlmdGluZyBhbmQgYWRkaW5nIHRvIHRoZSByZXNjdWU=
/// </summary>
[HardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
[FastJob]
public class BranchPrediction
{
    private const int N = 10000;
    private readonly int[] data;

    public BranchPrediction()
    {
        Random random = new(0);
        data = new int[N];
        for (int i = 0; i < N; i++)
        {
            data[i] = random.Next(256);
        }
    }

    [Benchmark]
    public int Branch()
    {
        int sum = 0;
        for (int i = 0; i < N; i++)
        {
            if (data[i] >= 128)
            {
                sum += data[i];
            }
        }

        return sum;
    }
}
