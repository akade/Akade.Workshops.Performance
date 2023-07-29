using Akade.Workshops.Performance.Api.Data;
using Akade.Workshops.Performance.Api.Infrastructure;
using System.Diagnostics.Metrics;

/// <summary>
/// Data Source (only swiss stations for year 2022):
/// 
/// Menne, M.J., I. Durre, B. Korzeniewski, S. McNeill, K. Thomas, X. Yin, S. Anthony, R. Ray, 
/// R.S.Vose, B.E.Gleason, and T.G.Houston, 2012: Global Historical Climatology Network - 
/// Daily (GHCN-Daily), Version 3.30
/// NOAA National Climatic Data Center. http://doi.org/10.7289/V5D21VHZ [2023-07-20].
/// 
/// More info in https://www.ncei.noaa.gov/pub/data/ghcn/daily/readme-by_year.txt & in https://www.ncei.noaa.gov/pub/data/ghcn/daily/readme.txt
/// </summary>
internal static class SourceDataLoader
{
    internal static async Task LoadAndSaveAsync(IHistoricalWeatherDataRepository repository)
    {
        IEnumerable<(string code, string name)> stations = LoadStations();

        foreach (string weatherData in File.ReadLines("SourceData/CH2022.csv"))
        {
            HistoricalWeatherData? parsedData = ParseWeatherData(weatherData, stations);
            if (parsedData != null)
            {
                await repository.SaveWeatherDataAsync(parsedData);
            }
        }
    }

    // Fields:
    // 0: ID = 11 character station identification code
    // 1: YEAR/MONTH/DAY = 8 character date in YYYYMMDD format(e.g. 19860529 = May 29, 1986)
    // 2: ELEMENT = 4 character indicator of element type
    // 3: DATA VALUE = 5 character data value for ELEMENT
    // 4: M-FLAG = 1 character Measurement Flag
    // 5: Q-FLAG = 1 character Quality Flag
    // 6: S-FLAG = 1 character Source Flag
    // 7: OBS-TIME = 4-character time of observation in hour-minute format(i.e. 0700 =7:00 am)
    private static HistoricalWeatherData? ParseWeatherData(string weatherData, IEnumerable<(string code, string name)> stations)
    {
        string[] parts = weatherData.Split(new[] { ',' });

        if (parts.Length < 7)
        {
            return null;
        }

        int year = int.Parse(parts[1][..4]);
        int month = int.Parse(parts[1][4..6]);
        int day = int.Parse(parts[1][6..8]);

        return new()
        {
            StationCode = parts[0],
            Station = stations.Single(x => x.code.Equals(parts[0])).name,
            Date = new DateOnly(year, month, day),
            ElementType = Enum.Parse<ElementType>(parts[2]),
            DataValue = ParseDataValue(parts),
            MFlag = ParseMFlag(parts[4]),
            QFlag = ParseQFlag(parts[5]),
            SFlag = ParseSFlag(parts[6]),
        };
    }

    private static double ParseDataValue(string[] parts)
    {
        return Enum.Parse<ElementType>(parts[2]) != ElementType.PRCP && Enum.Parse<ElementType>(parts[2]) != ElementType.SNOW ? double.Parse(parts[3]) / 10 : double.Parse(parts[3]);
    }

    private static MeasurementFlag ParseMFlag(string value)
    {
        try
        {
            return Enum.Parse<MeasurementFlag>(value);
        }
        catch
        {
            return MeasurementFlag.Blank;
        }
    }

    private static QualityFlag ParseQFlag(string value)
    {
        try
        {
            return Enum.Parse<QualityFlag>(value);
        }
        catch
        {
            return QualityFlag.Blank;
        }
    }

    private static SourceFlag ParseSFlag(string value)
    {
        try
        {
            return Enum.Parse<SourceFlag>(value);
        }
        catch
        {
            try
            {
                // try the numbers
                return Enum.Parse<SourceFlag>("_" + value);
            }
            catch
            {
                return SourceFlag.Blank;
            }

        }
    }

    internal static IEnumerable<(string code, string name)> LoadStations()
    {
        return File.ReadLines("SourceData/CHStationList.csv")
                   .Select(x => x.Split(','))
                   .Where(x => x.Length >= 5)
                   .Select(x => (code: x[0], name: x[4]));
    }
}