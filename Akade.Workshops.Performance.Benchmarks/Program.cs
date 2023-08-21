global using Akade.Workshops.Performance.Benchmarks.Infrastrucuture;
using Akade.Workshops.Performance.Benchmarks.Introductory;
using BenchmarkDotNet.Running;

// Introductory
BenchmarkSwitcher.FromTypes(new[] { typeof(NETUpdatesLinq), typeof(NETUpdatesStringOperations), typeof(EFCoreUpdatesInsertion), typeof(EFCoreUpdatesReading) });

// "Required"
// BenchmarkSwitcher.FromTypes(new[] { typeof(EFCoreQueries), typeof(ExceptionsAsControlFlow), typeof(LazyLinqEnumeration), typeof(QuadraticScaling) });

// "Intermediate"
// BenchmarkSwitcher.FromTypes(new[] { typeof(EFCoreTracking), typeof(EmptyCollections), typeof(EnumParsing), typeof(PrematureOptimization), typeof(Serialization), typeof(StringComparisonBenchmarks), typeof(StringConcatenations) });

// "Advanced"
// BenchmarkSwitcher.FromTypes(new[] { typeof(AdvancedStringParsing), typeof(BoxingAndSpecialization), typeof(BranchPrediction), typeof(CpuCache), typeof(Intrinsics) });

// Normally you would do this:
// BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);