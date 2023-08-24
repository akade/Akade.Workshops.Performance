using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Akade.Workshops.Performance.Benchmarks.Advanced;

/// <summary>
/// A simple task or is it? Make the summation as fast as you can!
/// Hints:
/// - VGhlcmUgYXJlIGRpZmZlcmVudCBraW5kIG9mIGxvb3BzLCB3aGljaCBpcyB0aGUgZmFzdGVzdD8=
/// - VGFrZSBhIGxvb2sgYXQgdmVjdG9yaXplZCAoU0lNRCkgb3BlcmF0aW9ucyBpbiBTeXN0ZW0uTnVtZXJpY3MuIFlvdSB3aWxsIHByb2JhYmx5IG5lZWQgdG8gZ29vZ2xlIGhvdyB0byBkbyBpdC4=
/// </summary>
[FastJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net70)]
[DisassemblyDiagnoser]
public class Intrinsics
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


