using Akade.Workshops.Performance.Benchmarks.Introductory;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Akade.Workshops.Performance.Benchmarks.Intermediate;

/// <summary>
/// (Json) Serialization is an important part of any web pipeline. Here, for serialization Newtonsoft.Json is used, which
/// *was* the defacto standard for a long time. Are other serializers faster?
/// Hints
/// - U3lzdGVtLlRleHQuSnNvbg==
/// </summary>
[FastJob]
[MemoryDiagnoser]
public class Serialization
{
    private readonly DataEntry[] entries = Enumerable.Range(0, 100)
                                                     .Select(i => new DataEntry() { Id = i, Value = i * 2 })
                                                     .ToArray();

    [Benchmark]
    public void SerializeAndDeserialize()
    {
        JsonConvert.DeserializeObject<DataEntry[]>(JsonConvert.SerializeObject(entries));
    }
}
