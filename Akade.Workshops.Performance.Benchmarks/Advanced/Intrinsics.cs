using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Akade.Workshops.Performance.Benchmarks.Advanced;

/// <summary>
/// A simple task or is it? Make the summation as fast as you can!
/// Hints:
/// - There are differnt kind of loops, which is the fastest?
/// - Can Modern CPUs do more than scalar operations?
/// - 
/// </summary>
[FastJob]
[DisassemblyDiagnoser]
public class Intriniscs
{
    private readonly int[] array = Enumerable.Range(0, 2560).ToArray();


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
}


