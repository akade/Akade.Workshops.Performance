using BenchmarkDotNet.Attributes;
using System.Collections.Generic;

namespace Akade.Workshops.Performance.Benchmarks.DataStructures;

/// <summary>
/// This benchmark demonstrates how the Big-O-Notation matters and small number
/// of data entries (as commonly found during development) can be very deceiving
/// Can you improve on it?
/// - Do not change GetFirstList or GetSecondList, they'd represent an api call or similar
/// - Do not calculate the result, it should work with arbitrary data
/// - Hint 1: SXMgdGhlcmUgYXdheSB0byBwcmVwYXJlIGZhc3RlciBsb29rdXAgb24gdGhlIGRhdGEgcmV0dXJuZWQgYnkgR2V0U2Vjb25kTGlzdCgpPw==
/// - Hint 2: Q2hlY2sgYWxsIHRoZSBjb2xsZWN0aW9ucyBhdmFpbGFibGUgaW4gU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuIE9uZSBvZiB0aGVtIGlzIGEgc29sdXRpb24gZm9yIHRoZSBwcm9ibGVtLg==
/// </summary>
[FastJob]
public class QuadraticScaling
{
    [Params(5, 100, 1000)]
    public int NumberOfElements { get; set; }

    [Benchmark]
    public int Count_all_numbers_who_are_also_in_the_second_list()
    {
        return GetFirstList().Where(x => GetSecondList().Contains(x)).Count(); 
    }

    private int[] GetFirstList() => Enumerable.Range(0, NumberOfElements).ToArray();
    private int[] GetSecondList() => Enumerable.Range(0, NumberOfElements).Select(x => x * 2).ToArray();
}
