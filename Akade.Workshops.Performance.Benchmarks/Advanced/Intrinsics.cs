using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Akade.Workshops.Performance.Benchmarks.Advanced;

/// <summary>
/// Which one is the fastest method? Can you improve upon it?
/// </summary>
[FastJob]
[DisassemblyDiagnoser]
public class Intriniscs
{
    private readonly int[] array = Enumerable.Range(0, 2560).ToArray();


    [Benchmark]
    public int SumLinq()
    {
        return array.Sum();
    }

    [Benchmark]
    public int SumFor()
    {
        int sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum;
    }

    [Benchmark]
    public int SumForeach()
    {
        int sum = 0;
        foreach (int val in array)
        {
            sum += val;
        }
        return sum;
    }

    [Benchmark]
    public int SumSIMD()
    {
        ReadOnlySpan<int> span = array;

        int lastIndexOfBlockk = array.Length - array.Length % Vector<int>.Count;
        int pos = 0;
        Vector<int> result = Vector<int>.Zero;
        while (pos < lastIndexOfBlockk)
        {
            result += new Vector<int>(span.Slice(pos));
            pos += Vector<int>.Count;
        }

        int actualResult = 0;
        for (int i = 0; i < Vector<int>.Count; i++)
        {
            actualResult += result[i];
        }

        while (pos < span.Length)
        {
            actualResult += span[pos];
            pos++;
        }

        return actualResult;
    }
}


