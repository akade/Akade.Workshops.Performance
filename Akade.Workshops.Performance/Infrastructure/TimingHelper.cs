using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Akade.Workshops.Performance.Api.Infrastructure;

/// <summary>
/// Helps you to easily instrument certain tasks
/// </summary>
public static class TimingHelper
{
    public static DisposableTimingStruct RecordTiming(this Histogram<double> histogram)
    {
        return new DisposableTimingStruct(histogram, Stopwatch.GetTimestamp());
    }

    public readonly struct DisposableTimingStruct : IDisposable
    {
        private readonly Histogram<double> _histogram;
        private readonly long _startTimestamp;

        public DisposableTimingStruct(Histogram<double> histogram, long startTimestamp) : this()
        {
            _startTimestamp = startTimestamp;
            _histogram = histogram;
        }

        public void Dispose()
        {
            long difference = Stopwatch.GetTimestamp() - _startTimestamp;
            _histogram.Record((difference / (double)Stopwatch.Frequency));
        }
    }
}


