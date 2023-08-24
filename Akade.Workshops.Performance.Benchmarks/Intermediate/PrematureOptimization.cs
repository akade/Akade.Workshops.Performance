using BenchmarkDotNet.Attributes;

namespace Akade.Workshops.Performance.Benchmarks.Intermediate;


/// <summary>
/// This is a famous example for unvalidated assumptions and not measuring. In the following
/// code, a small "optimization" happened that actually hurts. Can you make the compiled
/// code size smaller and explain what is going on?
/// Hint:
/// - V2hhdCBoYXBwZW5zIHdoZW4geW91IGFjY2VzcyBhbiBhcnJheT8=
/// - VGhlIEppdCBjYW4gb2Z0ZW4gb3B0aW1pemUgdGhlIGJvdW5kcyBjaGVja2luZyBhd2F5LCB3aHkgbm90IGluIHRoYXQgY2FzZT8=
/// - SXQgKGN1cnJlbnRseSkgaGFzIG5vIGtub3dsZWRnZSBhYm91dCB3aGV0aGVyIHRoZSBjb25zdGFudCBtYXRjaGVzIF92YWx1ZXMuTGVuZ3Ro
/// </summary>
[FastJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net70)]
[DisassemblyDiagnoser]
public class PrematureOptimization
{
    const int count = 100;
    readonly int[] _values = Enumerable.Range(1, count).ToArray();

    [Benchmark]
    public int Sum()
    {
        int sum = 0;
        for(int i = 0; i < count; i++)
        {
            sum += _values[i];
        }
        return sum;
    }

    
}
