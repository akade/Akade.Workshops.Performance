using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Akade.Workshops.Performance.Benchmarks;

/// <summary>
/// (Json) Serialization is an important part of any web pipeline. Here, for serializatino Newtonsoft.Json is used, which
/// *was* the defacto standard for a long time. Are other serializer faster?
/// Hints
/// - U3lzdGVtLlRleHQuSnNvbg==
/// - WW91IGNhbiwgaW4gZmFjdCwgbWFrZSBpdCBldmVuIGEgYml0IGZhc3RlciB3aXRob3V0IHVzaW5nIGFuIGV4dGVybmFsIGxpYnJhcnkuIFRha2UgYSBsb29rIGF0IHNvdXJjZSBnZW5lcmF0aW9uIGFuZCBqc29uLg==
/// </summary>
[FastJob]
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
