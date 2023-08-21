using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Text;

namespace Akade.Workshops.Performance.Benchmarks.Introductory;

/// <summary>
/// Updating to the latest and greates can help you to have some free lunch throughout the enitre stack. Run the benchmarks and see for yourself.
/// If you want to read more about what they did, there are the famous and excellent posts by Stephen Toub (very advanved and extremly long material):
/// https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/
/// https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-6/
/// Credits to the benchmarks also go to Stephen Toub

/// You can easily compare across framework options, for example if you also target Mono (i.e. MAUI) or WASM (i.e. Blazor) or Native AOT...
/// </summary>
[FastJob(runtimeMoniker: RuntimeMoniker.Net60, baseline: true)]
[FastJob(runtimeMoniker: RuntimeMoniker.Net70)]
public class NETUpdatesLinq
{
    [GlobalSetup]
    public void Setup() => _source = Enumerable.Range(1, Length).Select(x => (double)x).ToArray();

    [Params(4, 1024)]
    public int Length { get; set; }

    private IEnumerable<double> _source = null!;


    [Benchmark]
    public double Average() => _source.Average();
}
