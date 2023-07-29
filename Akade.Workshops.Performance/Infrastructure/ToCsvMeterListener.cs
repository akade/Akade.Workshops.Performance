using System.Diagnostics.Metrics;
using System.Text;

namespace Akade.Workshops.Performance.Api.Infrastructure;

/// <summary>
/// In a real application, you would publish it to Prometheus (and us it in Grafana) or have it collected by Azure...
/// https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics-collection#view-metrics-in-grafana-with-opentelemetry-and-prometheus
/// For the workshop, it writes every measurement into "counters.csv"
/// You can watch it while the application is running with "Get-Content -Path counters.csv -Wait" or "tail counters.csv".
///There is also dotnet-counters that you can use: https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-counters
/// </summary>
public sealed class ToCsvMeterListener : IDisposable
{
    private readonly MeterListener _listener;
    private readonly FileStream _fileStream;

    public ToCsvMeterListener()
    {
        _listener = new MeterListener()
        {
            InstrumentPublished = OnInstrumentPublished,
            MeasurementsCompleted = OnMeasurementsCompleted
        };

        _listener.SetMeasurementEventCallback<double>(OnMeasurement);
        _fileStream = new FileStream("counters.csv", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
    }

    private void OnMeasurement(Instrument instrument, double measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
    {
        Span<byte> buffer = stackalloc byte[1024];
        int length = Encoding.UTF8.GetBytes($"{DateTime.UtcNow:yyyy-mm-dd-HH-mm-ss},{instrument.Meter.Name},{instrument.Name},{measurement},{instrument.Unit}{Environment.NewLine}", buffer);
        _fileStream.Write(buffer[..length]);
        _fileStream.Flush();
    }

    private void OnMeasurementsCompleted(Instrument instrument, object? arg2)
    {
        _ = _listener.DisableMeasurementEvents(instrument);
    }

    private void OnInstrumentPublished(Instrument instrument, MeterListener listener)
    {
        _listener.EnableMeasurementEvents(instrument);
    }

    public void Dispose()
    {
        _listener.Dispose();
        _fileStream.Dispose();
    }

    public void Start()
    {
        _listener.Start();
    }
}
