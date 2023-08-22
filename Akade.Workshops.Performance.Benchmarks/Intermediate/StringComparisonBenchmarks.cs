using BenchmarkDotNet.Attributes;

namespace Akade.Workshops.Performance.Benchmarks.Intermediate;

/// <summary>
/// There are different ways to compare text. Can you make the following faster? And reduce memory allocations?
/// Hints:
/// - VG9Mb3dlcigpIHJldHVybnMgYSBmcmVzaGx5IGFsbG9jYXRlZCBzdHJpbmc=
/// - U3RyaW5nIGNvbXBhcmlzb24gaXMgY3VsdHVyZSBzZW5zaXRpdmU=
/// </summary>
[FastJob]
[MemoryDiagnoser]
public class StringComparisonBenchmarks
{

    [Benchmark]
    public bool AreEqual()
    {
        return "AAAAaaBBBCCCc".ToLower() == "AAAAaaBBBCCCc".ToLower();
    }

}
