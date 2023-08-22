using Newtonsoft.Json;

namespace Akade.Workshops.Performance.Client;

public sealed class HistoricalWeatherClient : IDisposable
{
    private readonly HttpClient _client;
    public HistoricalWeatherClient(string baseUrl)
    {
        _client = new()
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public async Task<Station[]> GetStationsAsync()
    {
        string apiResponse = await _client.GetStringAsync("/HistoricalWeather/stations");
        return JsonConvert.DeserializeObject<Station[]>(apiResponse) ?? throw new InvalidOperationException("Failed to deserialize stations");
    }

    public async Task<HistoricalData> GetDataForStationAsync(Station station, DateOnly date)
    {
        string apiResponse = await _client.GetStringAsync($"/HistoricalWeather/stations/{station.Code}/{date:yyyy-MM-dd}");
        return JsonConvert.DeserializeObject<HistoricalData>(apiResponse) ?? throw new InvalidOperationException("Failed to deserialize data");
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
