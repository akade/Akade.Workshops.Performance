# Akade.Workshops.Performance

> Workshop at a company internal conference. Might be workable on your own, but is augmented with
an introduction and on-site support.


This Readme will guide you through the workshop.

## Overview

The goal of this workshop is to make the sample application runnable.
It is written with bad performance in mind :snail:. Note, that 
while this is intentionally slow code, *every single issue has 
been encountered in the wild!*

The app consists of
- A ASP.Net Core Api
- Sqlite-DB with EF (with simulated network latency)
- A console app as a client application

and does the following
- Reading in historical weather data (CH 2022) at startup
- Query the data via the console application
 
The workshop is designed for different levels of experience and details.
There are benchmarks in `Akade.Workshops.Performance.Benchmarks` that 
are puzzles to solve. Each "solved" benchmark is something that you 
can use to optimize the app. Additionally, there are more advanced benchmarks,
that cover interesting topics if you are more experienced with performance
related coding, however, they are less relevant to the application and more niche.

*The first step will be to include metrics, then you will most likely
switch between profiling to find out slow parts of the code and "solving"
benchmarks.*

## 1. Metrics

In order to be aware of your apps performance characteristics, it is advisable to
continuously track performance. This can either be done with regularly running
benchmarks or more common, with metrics. Benchmarks, while important, are often 
synthetic while Metrics can measure real-world actions.

### 1.1 Add metrics

Possible places:
- `SourceDataLoader.LoadAndSaveAsync(...)`
- `HistoricalWeatherController`-Methods
- ...

Note: Don't try to do full instrumentation (i.e. every method), one or two are enough for the workshop. 
And also in practice, there is usually not much value in instrumenting everything.

You can create a new meter as follows. Histograms are usually used for timings:

```csharp
static Meter s_meter = new Meter("SampleSourceName", "1.0.0");
static Histogram<double> s_orderProcessingTimeSeconds =
    s_meter.CreateHistogram<double>("order-processing-time");
```

Recording a timing then works like this:

```csharp
s_orderProcessingTimeSeconds.Record(elapsedSeconds);

// However, for the workshop, there already is a helper that includes the time measuring itself:
using(TimingHelper.RecordTiming(s_orderProcessingTimeSeconds))
{
    // do work
}
```

### 1.2 Observe

For observing, usually tools like Prometheus and Grafana are used. For the limited amount of time,
the workshop includes `ToCsvMeterListener`, which listens to all measurements and writes
it to disk. 
Monitoring can be done using (PS): `Get-Content -Path counters.csv -Wait" or "tail counters.csv`.

## 2. Profiling

The following guide is for the VS Profiler (Debug > Performance Profiler, ALT + F2).

Most of the time, you want to know where CPU time is spent. CPU Usage is the tool for that. 
It works by sampling, which is short for checking what code is currently running at a regular interval.
The default is 1000 samples/second.

![VSProfiler1](/docs/images/VSProfiler_1.png)

- Select CPU Usage and the Api as a startup project. 
- Press start, do what you want to measure. 
  - Wait for the loading to finish (or let it run for a while, no need to wait until the end)
  - Make a few request with the console app.
    Tip: Right-click on the Client *.csproj > Debug > Start New Instance

Stop the app, the profiler will automatically collect the data:
![VSProfiler1](/docs/images/VSProfiler_2.png)

*Be patient, this might take a few seconds*

You'll be presented by the following overview page:
![VSProfiler1](/docs/images/VSProfiler_3.png)

Top Functions will give you a hint, as well as the hot path and the pie chart. My usual flow then is to click 
on a function of your own code within the *hot path*. In the next window, you can view your data differently.
I recommend to go with *Flame Graph* or with *Call Tree*-Views:

![VSProfiler1](/docs/images/VSProfiler_4.png)

Use **Total CPU** and **Self CPU** to find interesting methods. *Total* is the % of time within the method and
all methods that are called. *Self* is without called methods. Double click on them to see your code annotated.

> Hint: If no source code get's displayed, try the following: Right click on any function -> *Load All Symbols*

## 3. Benchmarking

Benchmarking is used to find out what code is truly faster and is also helpful in figuring out some more advanced
characteristics of your code such as memory allocations, inlining or even the jitted assembly code.

### Benchmark Puzzles

Each benchmark has a summary that describes what the goal of the puzzle:

```csharp
/// <summary>
/// (Json) Serialization is an important part of any web pipeline. Here, for serialization Newtonsoft.Json is used, which
/// *was* the defacto standard for a long time. Are other serializers faster?
/// Hints
/// - U3lzdGVtLlRleHQuSnNvbg==
/// - WW91IGNhbiwgaW4gZmFjdCwgbWFrZSBpdCBldmVuIGEgYml0I...
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
```

Each method marked with `[Benchmark]` is a benchmark itself. Usually, you'd want to write an additional method 
that uses the alternative, presumably faster, implementation. Alternatively, you can overwrite the method's 
content, if you do not need the comparison, to save time within the workshop. Better suited if, for example, you
want to minimize allocations while comparison is often required for accurately judging the execution time.

Running a benchmark is simple:
- Select the *Benchmarks* Project as startup
- Select *Release*-Configuration
- Start *without* debugger (CTRL+F5)

In this workshop, there are different benchmarks to solve. They are categorized as follows:

### Introductory
Start with these benchmarks. They are there to illustrate certain points and are ready to run.

### Required
This are benchmark puzzles (see below) that illustrate issues that are probably necessary to fix
within the sample app to get acceptable performance.

**Try to match the results of profiling output with one of these benchmarks.**

### Intermediate
Benchmarks puzzles that solve common but less impacting (for this specific scenario) issues. 

### Advanced
Benchmarks puzzles that are much more niche and usually reserved for the absolute hot paths in extremely
performance critical applications. Such optimizations might require more compromises with readability,
and hence, should be used sparingly and deliberately. However, they are nice to play with!
