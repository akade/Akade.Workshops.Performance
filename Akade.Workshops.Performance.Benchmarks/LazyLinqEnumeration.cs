﻿
/* Unmerged change from project 'Akade.Workshops.Performance.Benchmarks (net7.0)'
Before:
using BenchmarkDotNet.Attributes;
After:
using Akade;
using Akade.Workshops;
using Akade.Workshops.Performance;
using Akade.Workshops.Performance.Benchmarks;
using Akade.Workshops.Performance.Benchmarks;
using Akade.Workshops.Performance.Benchmarks.DataStructures;
using BenchmarkDotNet.Attributes;
*/
using BenchmarkDotNet.Attributes;

namespace Akade.Workshops.Performance.Benchmarks;

/// <summary>
/// This benchmark demonstrates how linq is evaluated "on demand".
/// Can you make it faster?
/// - Do not change LoadDataFromServer
/// - Hint: 
/// </summary>
[FastJob(unrollFactor: 1)]
public class LazyLinqEnumeration
{
    [Benchmark]
    public double CalculateStandardDeviation()
    {
        var dataFromServer = LoadDataFromServer();
        // sqrt{sum[(x-x_avg)^2]/(n-1)}
        double sumOfSquaredDifferences = dataFromServer.Sum(x => Math.Pow(x - dataFromServer.Average(), 2));
        return Math.Sqrt(sumOfSquaredDifferences / (dataFromServer.Count() - 1));
    }

    private static IEnumerable<int> LoadDataFromServer()
    {
        Thread.Sleep(5); // costly remote operation
        for (int i = 0; i < 5; i++)
        {
            yield return i;
        }
    }
}