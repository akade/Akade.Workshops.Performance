using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace Akade.Workshops.Performance.Benchmarks.Advanced;

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

    [Benchmark]
    public int Branchless()
    {
        int sum = 0;
        for (int i = 0; i < N; i++)
        {
            int t = (data[i] - 128) >> 31;
            sum += ~t & data[i];
        }
        return sum;
    }
}
