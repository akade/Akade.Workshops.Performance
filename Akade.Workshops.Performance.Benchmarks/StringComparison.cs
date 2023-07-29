using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akade.Workshops.Performance.Benchmarks;

/// <summary>
/// There are different ways to compare text, culture dependent, lower case. Compare them and see which is the fastest
/// </summary>
[FastJob]
public class StringComparisonBenchmarks
{

    [Benchmark]
    public bool AreEqual()
    {
        return "AAAAaaBBBCCCc".ToLower() == "AAAAaaBBBCCCc".ToLower();
    }

    [Benchmark]
    public bool AreEqualInvariant()
    {
        return "AAAAaaBBBCCCc".ToLowerInvariant() == "AAAAaaBBBCCCc".ToLowerInvariant();
    }

}
