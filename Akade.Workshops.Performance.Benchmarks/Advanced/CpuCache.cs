using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace Akade.Workshops.Performance.Benchmarks.Advanced;


/// <summary>
/// Multidimensional arrays are slower than jagged ones
/// Jagged ones are slower than a single continous bit
/// Continous memory access is faster than random (less cache misses)
/// </summary>
[HardwareCounters(HardwareCounter.CacheMisses)]
[FastJob]
public class CpuCache
{
    private const int N = 1000;
    public int[][] _values;
    public int[,] _values2;
    public int[] _values3;

    public CpuCache()
    {
        Random r = new(42);
        _values = new int[N][];
        _values2 = new int[N, N];
        _values3 = new int[N * N];
        for (int x = 0; x < N; x++)
        {
            _values[x] = new int[N];
            for (int y = 0; y < N; y++)
            {
                _values3[(x * N) + y] = _values[x][y] = _values2[x, y] = r.Next();

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
                sum += _values[x][y];
            }
        }

        return sum;
    }

    [Benchmark]
    public long Sum2()
    {
        long sum = 0;

        for (int x = 0; x < N; x++)
        {
            for (int y = 0; y < N; y++)
            {
                sum += _values[y][x];
            }
        }

        return sum;
    }

    [Benchmark]
    public long Sum3()
    {
        long sum = 0;

        for (int x = 0; x < N; x++)
        {
            for (int y = 0; y < N; y++)
            {
                sum += _values2[x, y];
            }
        }

        return sum;
    }

    [Benchmark]
    public long Sum4()
    {
        long sum = 0;

        for (int x = 0; x < N; x++)
        {
            for (int y = 0; y < N; y++)
            {
                sum += _values2[y, x];
            }
        }

        return sum;
    }

    [Benchmark]
    public long Sum5()
    {
        long sum = 0;

        for (int x = 0; x < N; x++)
        {
            for (int y = 0; y < N; y++)
            {
                sum += _values3[x * N + y];
            }
        }

        return sum;
    }

    [Benchmark]
    public long Sum6()
    {
        long sum = 0;

        for (int x = 0; x < N; x++)
        {
            for (int y = 0; y < N; y++)
            {
                sum += _values3[y * N + x];
            }
        }

        return sum;
    }

}
