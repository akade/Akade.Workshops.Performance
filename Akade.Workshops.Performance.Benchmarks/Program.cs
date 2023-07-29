global using Akade.Workshops.Performance.Benchmarks.Infrastrucuture;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
