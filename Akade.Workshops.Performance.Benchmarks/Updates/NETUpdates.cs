using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Text;

namespace Akade.Workshops.Performance.Benchmarks.Updates;

// Updating to the latest and greates can help you to have some free lunch throughout the enitre stack. Run the benchmark and see for yourself.
// If you want to read more about what they did are the famous and excellent posts by Stephen Toub (very advanved and extremly long material):
// https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/
// https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-6/
// Credits to the benchmarks also go to Stephen Toub

// You can easily compare across framework options, for example if you also target Mono (i.e. MAUI) or WASM (i.e. Blazor) or Native AOT...
// Note that FastJob is customized for the workshop to perform faster benchmarking traded for less accuracy
[FastJob(runtimeMoniker: RuntimeMoniker.Net60, baseline: true)]
[FastJob(runtimeMoniker: RuntimeMoniker.Net70)]
public class NETUpdates_Linq
{
    [GlobalSetup]
    public void Setup() => _source = Enumerable.Range(1, Length).Select(x => (double)x).ToArray();

    [Params(4, 1024)]
    public int Length { get; set; }

    private IEnumerable<double> _source = null!;
    

    [Benchmark]
    public double Average() => _source.Min();
}

[FastJob(runtimeMoniker:RuntimeMoniker.Net60, baseline: true)]
[FastJob(runtimeMoniker:RuntimeMoniker.Net70)]
public class NETUpdates_StringOperations
{
    private readonly string _value = "https://dot.net";

    [Benchmark]
    public bool IsHttps_Ordinal() => _value.StartsWith("https://", StringComparison.Ordinal);

    [Benchmark]
    public bool IsHttps_OrdinalIgnoreCase() => _value.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

    private readonly byte[] _data = Encoding.UTF8.GetBytes(@"
    Shall I compare thee to a summer's day?
    Thou art more lovely and more temperate:
    Rough winds do shake the darling buds of May,
    And summer's lease hath all too short a date;
    Sometime too hot the eye of heaven shines,
    And often is his gold complexion dimm'd;
    And every fair from fair sometime declines,
    By chance or nature's changing course untrimm'd;
    But thy eternal summer shall not fade,
    Nor lose possession of that fair thou ow'st;
    Nor shall death brag thou wander'st in his shade,
    When in eternal lines to time thou grow'st:
    So long as men can breathe or eyes can see,
    So long lives this, and this gives life to thee.
    ");

    [Benchmark]
    public string ToBase64String() => Convert.ToBase64String(_data);
}