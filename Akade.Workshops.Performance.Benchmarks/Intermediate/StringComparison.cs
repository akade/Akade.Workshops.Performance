using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akade.Workshops.Performance.Benchmarks.Intermediate;

/// <summary>
/// There are different ways to compare text, culture dependent, lower case. Compare them and see which is the fastest & most memory efficient ones.
/// </summary>
[FastJob]
[MemoryDiagnoser]
public class StringComparisonBenchmarks
{

    [Benchmark]
    public bool AreEqual()
    {
        return "AAAAaaBBBCCCc".ToLower() == "AAAAaaBBBCCCc".ToLower();
    }

}
