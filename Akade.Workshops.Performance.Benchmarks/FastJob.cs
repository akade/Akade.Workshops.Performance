using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace Akade.Workshops.Performance.Benchmarks;

/// <summary>
/// Faster benchmarks trading faster execution time for less accurate results (helps with the limited time we have in a workshop)
/// - Relative error of 10% instead of 2
/// - Maximum iteration & warmup iteration count of 5
/// - Higher starting unroll factor
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = true)]
public class FastJobAttribute : JobConfigBaseAttribute
{

    public FastJobAttribute(RuntimeMoniker runtimeMoniker = RuntimeMoniker.Net70, string? id = null, bool baseline = false, int unrollFactor = 1024, int maxIterationCount = 5, string nugetPackages = "")
       : base(CreateJob(runtimeMoniker, id, baseline, unrollFactor, maxIterationCount, ParseNugetPackages(nugetPackages)))
    {

    }

    private static (string name, string version)[] ParseNugetPackages(string nugetPackages)
    {
        if (string.IsNullOrEmpty(nugetPackages))
        {
            return Array.Empty<(string, string)>();
        }

        return nugetPackages.Split(';')
                            .Select(x => x.Split(','))
                            .Select(x =>
                            {
                                if (x.Length != 2)
                                {
                                    throw new ArgumentOutOfRangeException(nameof(nugetPackages));
                                }

                                return (name: x[0].Trim(), version: x[1].Trim());
                            }).ToArray();
    }

    private static Job CreateJob(RuntimeMoniker runtimeMoniker, string? id, bool baseline, int unrollFactor, int maxIterationCount, (string name, string version)[] nugetPackages)
    {
        Job job = new Job(id ?? GetRuntime(runtimeMoniker).Name)
            .WithMaxRelativeError(0.1)
            .WithMinIterationCount(2)
            .WithMaxIterationCount(maxIterationCount)
            .WithMinWarmupCount(2)
            .WithMaxWarmupCount(maxIterationCount)
            .WithRuntime(GetRuntime(runtimeMoniker))
            .WithBaseline(baseline)
            .WithUnrollFactor(unrollFactor);

        if(unrollFactor == 1)
        {
            job = job.WithInvocationCount(1);
        }

        foreach ((string pckName, string pckVersion) in nugetPackages ?? Array.Empty<(string, string)>())
        {
            job = job.WithNuGet(pckName, pckVersion);
        }

        return job.Freeze();
    }

    private static CoreRuntime GetRuntime(RuntimeMoniker runtimeMoniker)
    {
        return runtimeMoniker switch
        {
            RuntimeMoniker.Net70 => CoreRuntime.Core70,
            RuntimeMoniker.Net60 => CoreRuntime.Core60,
            _ => throw new InvalidOperationException("Only Net 6 & 7 supported, use SimpleJob instead of FastJob for others")
        };
    }
}
