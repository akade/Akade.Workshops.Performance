using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace Akade.Workshops.Performance.Benchmarks.Intermediate;

/// <summary>
/// This benchmark is a classic. Can you make it faster? And reduce it's memory footprint?
/// 
/// Hints:
/// - RWFjaCBzdHJpbmcgY29uY2F0ZW5hdGlvbiAoKykgY3JlYXRlcyBhIGZyZXNobHkgYWxsb2NhdGVkIHN0cmluZw==
/// - U3RyaW5nQnVpbGRlcg==
/// - Q2FuIHlvdSByZXVzZSB0aGUgU3RyaW5nQnVpbGRlcj8=
/// </summary>
[FastJob]
[MemoryDiagnoser]
public class StringConcatenations
{
    private readonly string[] _strings = Enumerable.Range(0, 100).Select(x => x.ToString()).ToArray();

    [Benchmark]
    public string AddOperator()
    {
        string result = string.Empty;
        foreach (string s in _strings)
        {
            result += s;
        }
        return result;
    }
}
