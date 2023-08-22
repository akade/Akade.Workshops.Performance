namespace Akade.Workshops.Performance.Api.Data;

public class HistoricalWeatherData
{
    public string StationCode { get; set; } = string.Empty;
    public string Station { get; set; } = string.Empty;

    public DateOnly Date { get; set; }
    public ElementType ElementType { get; set; }
    public double DataValue { get; set; }
    public MeasurementFlag MFlag { get; set; }
    public QualityFlag QFlag { get; set; }
    public SourceFlag SFlag { get; set; }


}


