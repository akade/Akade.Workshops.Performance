using BenchmarkDotNet.Attributes;

namespace Akade.Workshops.Performance.Benchmarks.Intermediate;

/// <summary>
/// Enums are kind of weird in .NET (do you know why?). Can you make the parsing faster?
/// Hints:
/// - UGFyc2luZyB1c2VzIHJlZmxlY3Rpb24gYW5kIHJlZmxlY3Rpb24gaXMgc2xvdyEgQ2FuIHlvdSBkbyB3aXRob3V0Pw==
/// - RW51bXMgYXJlIGFsd2F5cyBiYWNrZWQgYnkgYSBudW1iZXI6IFBhcnNpbmcgYWx3YXlzIG5lZWRzIHRvIGNvbnNpZGVyIGJvdGggdGhlIG51bWVyaWMgYW5kIHRoZSB0ZXh0dWFsIHJlcHJlc2VudGF0aW9u
/// - RW51bXMgY2FuIGFsc28gYmUgRmxhZ3MsIGkuZS4gc2VwYXJhdGVkIHZhbHVlcw==
/// - RW51bXMgYXJlICh1bmZvcnR1bmF0ZWx5KSBub3QgcmVzdHJpY3RlZCB0byB0aGVpciBlbnVtZXJhdGlvbiB2YWx1ZXMgYnV0IGNhbiBiZSBhbnl0aGluZyB0aGVpciBiYWNraW5nIHR5cGUgYWxsb3dz
/// </summary>
[FastJob]
public class EnumParsing
{
    [Params("Not Available", nameof(MyTestEnum.B))]
    public string Value { get; set; } = "";

    [Benchmark(Baseline = true)]
    public bool ParseEnum()
    {
        return Enum.TryParse<MyTestEnum>(Value, out _);
    }
}

public enum MyTestEnum
{
    A,
    B
}
