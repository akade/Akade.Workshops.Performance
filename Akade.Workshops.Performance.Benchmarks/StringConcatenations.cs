using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace Akade.Workshops.Performance.Benchmarks;

[FastJob]
[MemoryDiagnoser]
public class StringConcatenations
{
    private readonly string[] _strings = Enumerable.Range(0, 100).Select(x => x.ToString()).ToArray();
    private readonly ObjectPool<StringBuilder> _stringBuilderPool = new DefaultObjectPoolProvider().CreateStringBuilderPool();

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

    [Benchmark]
    public string StringBuilder()
    {
        StringBuilder builder = new();
        foreach (string s in _strings)
        {
            builder.Append(s);
        }
        return builder.ToString();
    }

    [Benchmark]
    public string PooledStringBuilder()
    {
        StringBuilder builder = _stringBuilderPool.Get();

        try
        {
            foreach (string s in _strings)
            {
                builder.Append(s);
            }
            return builder.ToString();
        }
        finally
        {
            _stringBuilderPool.Return(builder);
        }
    }
}
