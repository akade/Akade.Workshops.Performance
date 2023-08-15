global using Akade.Workshops.Performance.Benchmarks.Infrastrucuture;
using BenchmarkDotNet.Running;

// TODO: init with types. I.e. Intro, Required, Intermediate & Advanced
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);


