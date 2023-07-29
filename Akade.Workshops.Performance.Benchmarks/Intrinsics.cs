using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Akade.Workshops.Performance.Benchmarks;

/// <summary>
/// Can you spezialize the count method for int collections an in a way that is source compatible (i.e. exact same code at caller location)
/// and does not increase the code size of the exported method? 
/// 
/// Hints:
/// - It is solveable without commenting out [MethodImpl(MethodImplOptions.NoInlining)]. But you also experiment with it: You will observe a huge difference. Can you explain what happens?
/// </summary>
[FastJob]
[DisassemblyDiagnoser]
public class Intriniscs
{
    private readonly int[] array = Enumerable.Range(0,2560).ToArray();


    [Benchmark]
    public int SumLinq()
    {
        return array.Min();
    }

    [Benchmark]
    public int SumFor()
    {
        int sum = 0;    
        for(int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum;
    }

    [Benchmark]
    public int SumForeach()
    {
        int sum = 0;
        foreach(int val in array)
        {
            sum += val;
        }
        return sum;
    }

    [Benchmark]
    public int SumSIMD()
    {
        ReadOnlySpan<int> span = array;

        int lastIndexOfBlockk = array.Length - (array.Length % Vector<int>.Count);
        int pos = 0;
        Vector<int> result = Vector<int>.Zero;
        while (pos < lastIndexOfBlockk)
        {
            result += new Vector<int>(span.Slice(pos));
            pos += Vector<int>.Count;
        }

        int actualResult = 0;
        for(int i = 0; i < Vector<int>.Count; i++)
        {
            actualResult += result[i];
        }

        while(pos < span.Length)
        {
            actualResult += span[pos];
            pos++;
        }

        return actualResult;
    }
}


