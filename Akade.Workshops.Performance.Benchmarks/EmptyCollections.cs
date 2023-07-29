using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Akade.Workshops.Performance.Benchmarks;

/// <summary>
/// This benchmarks shows a simple, [almost] always recommendable easy win.
/// It is also an example on how BenchmarkDotNet currently recommends to evaluate IEnumerables and
/// on how the <see cref="MemoryDiagnoserAttribute"/> can be used to track allocations.
/// 
/// Can you explain the difference in performance?
/// - Note that all methods are equivalent from a caller's perspective.
/// </summary>
[FastJob]
[MemoryDiagnoser]
public class EmptyCollections
{
    private readonly Consumer consumer = new();

    [Benchmark]
    public void EmptyEnumerable() { EmptyEnumerableImpl().Consume(consumer); }

    [Benchmark]
    public void EmptyArray() { EmptyArrayImpl().Consume(consumer); }
    [Benchmark]
    public void NewArray() { NewArrayImpl().Consume(consumer); }
    [Benchmark]
    public void NewList() { NewListImpl().Consume(consumer); }

    public IEnumerable<int> EmptyEnumerableImpl() { return Enumerable.Empty<int>(); }

    public IEnumerable<int> EmptyArrayImpl() { return Array.Empty<int>(); }
    public IEnumerable<int> NewArrayImpl() { return new int[0]; }
    public IEnumerable<int> NewListImpl() { return new List<int>(); }
}
