using BenchmarkDotNet.Attributes;
using System.Collections;
using System.Numerics;

namespace Akade.Workshops.Performance.Benchmarks.Advanced;


/// <summary>
/// Can you make the sum method faster and without any memory allocations while it still supports both floats and ints? Can you explain what happens?
/// Hints:
/// - V2hhdCBoYXBwZW5zIGlmIHlvdSBwdXQgYSB2YWx1ZSB0eXBlIGludG8gYSByZWZlcmVuY2UgdHlwZT8gQ2FuIHlvdSBhdm9pZCB0aGF0Pw==
/// - R2VuZXJpY3Mgd2lsbCBoZWxwIHlvdS4gSG93IGRvIHRoZXkgYXZvaWQgYm94aW5nPw==
/// - VGhlIEpJVCBlbGltaW5hdGVzIHVucmVhY2hhYmxlIGNvZGUgKHZhcmlhbnQgMSkgLyB0aGVyZSBpcyBhIG5ldyBmZWF0dXJlIGluIC5ORVQgNyB0aGF0IG1heSBoZWxwIHlvdSAodmFyaWFudCAyKQ==
/// - UmVtZW1iZXIsIHRoZSBlbnVtZXJhdG9yIGlzIGFsc28gYWxsb2NhdGVkIGZvciBJRW51bWVyYWJsZTw+LiBDYW4geW91IGF2b2lkIHRoYXQ/IChZb3Ugc3RpbGwgY2FuIGRvIGEgZm9yZWFjaCk=
/// </summary>
[FastJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net70)]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class BoxingAndSpecialization
{
    private readonly int[] _integers = Enumerable.Range(0, 1000).ToArray();
    private readonly float[] _floats = Enumerable.Range(0, 1000).Select(x => (float)x).ToArray();


    [Benchmark]
    public int Sum_of_ints()
    {
        return (int)Sum(_integers);
    }

    [Benchmark]
    public float Sum_of_floats()
    {
        return (float)Sum(_floats);
    }


    private static double Sum(IEnumerable values)
    {
        double sum = 0;

        foreach(var value in values)
        {
            sum += Convert.ToDouble(value);    
        }

        return sum;
    }
}
