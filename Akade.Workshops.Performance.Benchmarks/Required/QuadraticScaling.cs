using BenchmarkDotNet.Attributes;
using System.Collections.Generic;

namespace Akade.Workshops.Performance.Benchmarks.Required;

/// <summary>
/// This benchmark demonstrates how the Big-O-Notation matters and small number
/// of data entries (as commonly found during development) can be very deceiving
/// Can you improve on it?
/// Hints:
/// - Note: Do not change GetFirstList or GetSecondList, they'd represent an api call or similar
/// - Note: Do not calculate the result, it should work with arbitrary data
/// - SXMgdGhlcmUgYXdheSB0byBwcmVwYXJlIGZhc3RlciBsb29rdXAgb24gdGhlIGRhdGEgcmV0dXJuZWQgYnkgR2V0U2Vjb25kTGlzdCgpPw==
/// - Q2hlY2sgYWxsIHRoZSBjb2xsZWN0aW9ucyBhdmFpbGFibGUgaW4gU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuIE9uZSBvZiB0aGVtIGlzIGEgc29sdXRpb24gZm9yIHRoZSBwcm9ibGVtLg==
/// - SGFzaFNldCBhbGxvd3MgTygxKSBjb250YWlucywgc3RvcmUgdGhlIHJlc3VsdCBvZiBHZXRTZWNvbmRMaXN0IGluIG9uZQ==
/// </summary>
[FastJob]
public class QuadraticScaling
{
    /// <summary>
    /// Params instruct BenchmarkDotNet to run the Benchmark with different values.
    /// </summary>
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
