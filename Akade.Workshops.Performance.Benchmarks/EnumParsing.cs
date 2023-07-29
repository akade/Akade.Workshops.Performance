using BenchmarkDotNet.Attributes;

namespace Akade.Workshops.Performance.Benchmarks;

/// <summary>
/// Enums are kind of wierd in .NET (do you know why?). Can you make the parsing faster?
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
